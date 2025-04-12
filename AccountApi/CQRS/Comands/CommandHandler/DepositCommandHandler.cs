
using System.Text.Json;
using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class DepositCommandHandler : ICommandHandler<DepositCommand, bool>
    {
        private readonly IGenericRepository<Account> accountrepository;
        private readonly IGenericRepository<Event> eventrepository;

        public DepositCommandHandler(IGenericRepository<Account> Accountrepository, IGenericRepository<Event> Eventrepository)
        {
            accountrepository = Accountrepository;
            eventrepository = Eventrepository;
        }

        public async Task<bool> HandlerAsync(DepositCommand request)
        {
            var account = await LoadAccountAsync(request.AccountId);
            account.Deposit(request.Amount);
            await accountrepository.UpdateAsync(account);
            var events = new List<Event>();
            foreach (var item in account.Changes)
            {
                events.Add(new Event
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = DateTime.Now,
                    AggregateId = account.Id,
                    EventType = item.NameOf,
                    EventData = JsonSerializer.Serialize(item)
                });
            }
            await eventrepository.AddAsync(events);
            return true;
        }

        private async Task<Account> LoadAccountAsync(Guid accountId)
        {
            var items = await eventrepository.FindAsync(c => c.AggregateId == accountId);
            var events = new List<EventBase>();
            foreach (var item in items.OrderBy(e=>e.OccurredOn))
            {
                switch (item.EventType)
                {
                    case "Account":
                        var createdEvent = JsonSerializer.Deserialize<CreatedEvent>(item.EventData);
                        events.Add(new CreatedEvent() { initialBalance = createdEvent.initialBalance, Name = createdEvent.Name, NameOf = createdEvent.NameOf });
                        break;
                    case "Withdraw":
                        var withdrawnEvent = JsonSerializer.Deserialize<WithdrawnEvent>(item.EventData);
                        events.Add(new DepositedEvent() { Amount = withdrawnEvent.Amount, NameOf = withdrawnEvent.NameOf });
                        break;
                    case "Deposit":
                        var depositedEvent = JsonSerializer.Deserialize<DepositedEvent>(item.EventData);
                        events.Add(new DepositedEvent() { Amount = depositedEvent.Amount, NameOf = depositedEvent.NameOf });
                        break;
                }
            }
            var account = await accountrepository.GetByIdAsync(accountId);
            account.LoadFromEvents(events);
            return account;
        }
    }
}
