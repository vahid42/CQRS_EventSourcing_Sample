using AccountApi.Entities;
using AccountApi.Events;
using System.Text.Json;

namespace AccountApi.CQRS
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
                            Id = createdEvent.Id
                        });
                        break;
                    case "Withdraw":
                        var withdrawnEvent = JsonSerializer.Deserialize<WithdrawnEvent>(item.EventData);
                        events.Add(new WithdrawnEvent()
                        {
                            Amount = withdrawnEvent.Amount,
                            NameOf = withdrawnEvent.NameOf,
                            Version = withdrawnEvent.Version
                        });
                        break;
                    case "Deposit":
                        var depositedEvent = JsonSerializer.Deserialize<DepositedEvent>(item.EventData);
                        events.Add(new DepositedEvent()
                        {
                            Amount = depositedEvent.Amount,
                            NameOf = depositedEvent.NameOf,
                            Version = depositedEvent.Version
                        });
                        break;
                }
            }

            return events;
        }
    }
}
