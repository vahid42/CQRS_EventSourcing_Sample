using System.Text.Json;
using System.Text.Json.Serialization;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.Entities
{
    public class Account
    {
        private List<object> _changes = new List<object>();
        private Account() { }



        public Guid Id { get; private set; }
        public decimal Balance { get; private set; }
        public string Name { get; private set; }


        private Account(string name, decimal initialBalance)
        {
            Name = name;
            Id = Guid.NewGuid();
            Balance = initialBalance;
        }

        public async Task<Account> CreateAccountAsync(string name, decimal initialBalance, IGenericRepository<Event> eventStore)
        {
            var account = new Account(name, initialBalance);

            var createdEvent = new CreatedEvent { Name = name };
            account.ApplyChange(createdEvent);

            var eventEntity = account.CreateEventEntity(nameof(CreatedEvent), createdEvent);
            await eventStore.AddAsync(eventEntity);

            return account;
        }

        public async Task DepositAsync(decimal amount, IGenericRepository<Event> eventStore)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            Balance += amount;
            ApplyChange(new DepositedEvent { Amount = amount });

            var eventEntity = CreateEventEntity(nameof(DepositedEvent), new { Amount = amount });
            await eventStore.AddAsync(eventEntity);
        }

        public async Task WithdrawAsync(decimal amount, IGenericRepository<Event> eventStore)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
            ApplyChange(new WithdrawnEvent { Amount = amount });

            var eventEntity = CreateEventEntity(nameof(WithdrawnEvent), new { Amount = amount });
            await eventStore.AddAsync(eventEntity);
        }

        public void ClearUncommittedChanges() => _changes.Clear();
        public IEnumerable<object> GetChanges() => _changes;

        private void ApplyChange(object @event)
        {
            _changes.Add(@event);
        }

        private Event CreateEventEntity(string eventType, object eventData)
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.UtcNow,
                AggregateId = this.Id,
                EventType = eventType,
                EventData = JsonSerializer.Serialize(eventData)
            };
        }

        public void LoadFromEvents(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case CreatedEvent createdEvent:
                        Name = createdEvent.Name;
                        break;
                    case DepositedEvent depositedEvent:
                        Balance += depositedEvent.Amount;
                        break;
                    case WithdrawnEvent withdrawnEvent:
                        Balance -= withdrawnEvent.Amount;
                        break;
                }
            }
        }
    }
}
