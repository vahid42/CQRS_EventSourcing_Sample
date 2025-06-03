using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Events;
using System.Text.Json;

namespace AccountFlow.Api.V2.CQRS
{
    public static class Convertor
    {
        public static List<Event> ConvertAccountToEvent(Account account)
        {
            var events = new List<Event>();
            foreach (var item in account.Changes)
            {
                var @event = new Event
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = DateTime.Now,
                    AggregateId = account.Id,
                    EventType = item.NameOf,
                    EventVersion = item.Version,
                    

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

            return events;
        }

        public static List<EventBase> ConvertEventBaseToEvent(IEnumerable<Event> items)
        {
            var events = new List<EventBase>();
            foreach (var item in items.OrderBy(e => e.OccurredOn)
                )
            {
                switch (item.EventType)
                {
                    case "Account":
                        var createdEvent = JsonSerializer.Deserialize<CreatedEvent>(item.EventData);
                        events.Add(new CreatedEvent()
                        {
                            initialBalance = createdEvent.initialBalance,
                            Name = createdEvent.Name,
                            NameOf = createdEvent.NameOf,
                            Version = createdEvent.Version,
                            Created = createdEvent.Created,
                            IsActive = createdEvent.IsActive,
                            Id = createdEvent.Id,
                            RowVersion=createdEvent.RowVersion
                            


                        });
                        break;
                    case "Withdraw":
                        var withdrawnEvent = JsonSerializer.Deserialize<WithdrawnEvent>(item.EventData);
                        events.Add(new WithdrawnEvent()
                        {
                            Amount = withdrawnEvent.Amount,
                            NameOf = withdrawnEvent.NameOf,
                            Version = withdrawnEvent.Version,
                            RowVersion = withdrawnEvent.RowVersion
                        });
                        break;
                    case "Deposit":
                        var depositedEvent = JsonSerializer.Deserialize<DepositedEvent>(item.EventData);
                        events.Add(new DepositedEvent()
                        {
                            Amount = depositedEvent.Amount,
                            NameOf = depositedEvent.NameOf,
                            Version = depositedEvent.Version,
                            RowVersion = depositedEvent.RowVersion
                        });
                        break;
                }
            }

            return events;
        }

        public static AccountSnapshot ConvertAccountToSnapShot(Account account)
        {
            return new AccountSnapshot()
            {
                Id = Guid.NewGuid(),
                CurrentVersion = account.CurrentVersion,
                Balance = account.Balance,
                Created = account.Created,
                IsActive = account.IsActive,
                AccountId = account.Id,
                Name = account.Name,
            };

        }
        public static Account ConvertSnapShotToAccount(AccountSnapshot account)
        {

            return new Account(account.Name, account.Balance);
             

        }
    }
}
