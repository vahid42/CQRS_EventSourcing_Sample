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

        public Account(string name, decimal balance)
        {
            var @event = new CreatedEvent { Name = name, initialBalance = balance, NameOf = "Account" };
            ApplyChange(@event);
            changes.Add(@event);
          
        }
        public Guid Id { get; protected set; }
        public decimal Balance { get; protected set; }
        public string Name { get; protected set; }
        public bool IsActive { get; protected set; }
        public DateTime Created { get; protected set; }

        public List<EventBase> Changes => changes;


        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            var @event = new DepositedEvent { Amount = amount, NameOf = "Deposit" };
            ApplyChange(@event);
            changes.Add(@event);
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");

            var @event = new WithdrawnEvent { Amount = amount, NameOf = "Withdraw" };
            ApplyChange(@event);
            changes.Add(@event);
        }


        public void ClearChanges() => changes.Clear();

        private void ApplyChange(EventBase @event)
        {
            switch (@event)
            {
                case CreatedEvent createdEvent:
                    Id = Guid.NewGuid();
                    Name = createdEvent.Name;
                    Balance = createdEvent.initialBalance;
                    IsActive = true;
                    Created = DateTime.Now;
                    break;
                case DepositedEvent depositedEvent:
                    Balance += depositedEvent.Amount;
                    break;
                case WithdrawnEvent withdrawnEvent:
                    Balance -= withdrawnEvent.Amount;
                    break;
            }
        }

        public void LoadFromEvents(IEnumerable<EventBase> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case CreatedEvent createdEvent:
                        Name = createdEvent.Name;
                        Balance = createdEvent.initialBalance;
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
