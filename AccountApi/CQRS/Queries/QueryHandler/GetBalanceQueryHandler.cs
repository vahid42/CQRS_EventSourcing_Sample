
using System.Text.Json;
using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.CQRS.Queries.QueryHandler
{
    public class GetBalanceQueryHandler : IQueryHandler<GetBalanceQuery, decimal>
    {
        private IGenericRepository<Account> accountrepository;
        private IGenericRepository<Event> eventrepository;

        public GetBalanceQueryHandler(IGenericRepository<Account> Accountrepository, IGenericRepository<Event> Eventrepository)
        {
            this.accountrepository = Accountrepository;
            this.eventrepository = Eventrepository;
        }



        public async Task<decimal> HandlerAsync(GetBalanceQuery request)
        {

            var account = await LoadAccountAsync(request.AccountId);
            return account.Balance;
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
                        events.Add(new WithdrawnEvent() { Amount = withdrawnEvent.Amount, NameOf = withdrawnEvent.NameOf });
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
