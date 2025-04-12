using System.Text.Json;
using System.Text.Unicode;
using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, Guid>
    {
        private readonly IGenericRepository<Account> accountrepository;
        private readonly IGenericRepository<Event> eventrepository;

        public CreateCommandHandler(IGenericRepository<Account> Accountrepository, IGenericRepository<Event> Eventrepository)
        {
            accountrepository = Accountrepository;
            eventrepository = Eventrepository;
        }

        public async Task<Guid> HandlerAsync(CreateCommand request)
        {
            var account = new Account(request.Name, request.InitialBalance);
            //Adding amount for bonus
            account.Deposit(1000);
            var result = await accountrepository.AddAsync(account);

            var events = new List<Event>();
            foreach (var item in account.Changes)
            {
                var @event = new Event
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = DateTime.Now,
                    AggregateId = account.Id,
                    EventType = item.NameOf
                };

                switch (item.NameOf)
                {
                    case "Account":
                        @event.EventData = JsonSerializer.Serialize((CreatedEvent)item);                   
                        break;
                    case "Withdraw":
                        @event.EventData = JsonSerializer.Serialize((WithdrawnEvent)item);
                        break;
                    case "Deposit":
                        @event.EventData = JsonSerializer.Serialize((DepositedEvent)item);
                        break;
                }
                events.Add(@event);

            }
            await eventrepository.AddAsync(events);

            return result.Id;
        }
    }
}
