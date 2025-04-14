using System.Text.Json;
using System.Text.Json.Serialization;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.Entities
{
    public class Account
    {
        private List<EventBase> changes = new List<EventBase>();
        public Account() { }


        public Guid Id { get; protected set; }
        public decimal Balance { get; protected set; }
        public string Name { get; protected set; }
        public bool IsActive { get; protected set; }
        public DateTime Created { get; protected set; }
        public List<EventBase> Changes => changes;
        public int Version { get; protected set; }

        public Account(string name, decimal balance)
        {
            var @event = new CreatedEvent
            {
                Id = Guid.NewGuid(),
                Name = name,
                initialBalance = balance,
                NameOf = "Account",
                Version = 1,
                Created = DateTime.Now,
                IsActive = true
            };
            ApplyChange(@event);
            changes.Add(@event);

        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            var @event = new DepositedEvent { Amount = amount, NameOf = "Deposit", Version = Version + 1 };
            ApplyChange(@event);
            changes.Add(@event);
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");

            var @event = new WithdrawnEvent { Amount = amount, NameOf = "Withdraw", Version = Version + 1 };
            ApplyChange(@event);
            changes.Add(@event);
        }


        public void ClearChanges() => changes.Clear();
        public void LoadFromEvents(IEnumerable<EventBase> events)
        {
            foreach (var @event in events.OrderBy(c => c.Version))
            {
                ApplyEventToState(@event);
            }
        }

        private void ApplyChange(EventBase @event)
        {
            ValidateEventVersion(@event);
            ApplyEventToState(@event);
            Version = @event.Version;
        }


        private void ValidateEventVersion(EventBase @event)
        {
            if (@event is CreatedEvent && @event.Version != 1)
                throw new InvalidOperationException("CreatedEvent must have Version = 1");

            if (!(@event is CreatedEvent) && @event.Version != this.Version + 1)
                throw new InvalidOperationException($"Event out of order. Expected: {this.Version + 1}, Received: {@event.Version}");
        }


        private void ApplyEventToState(EventBase @event)
        {
            switch (@event)
            {
                case CreatedEvent createdEvent:
                    Id = createdEvent.Id;
                    Name = createdEvent.Name;
                    Balance = createdEvent.initialBalance;
                    IsActive = createdEvent.IsActive;
                    Created = createdEvent.Created;
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
