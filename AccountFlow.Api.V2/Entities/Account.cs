using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using AccountFlow.Api.V2.Events;
using AccountFlow.Api.V2.Repository;

namespace AccountFlow.Api.V2.Entities
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
        public int CurrentVersion { get; protected set; }


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
                IsActive = true,
                RowVersion = Guid.NewGuid(),

            };
            ApplyChange(@event);
            changes.Add(@event);

        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            var @event = new DepositedEvent { Amount = amount, NameOf = "Deposit", Version = CurrentVersion + 1};
            ApplyChange(@event);
            changes.Add(@event);
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");

            var @event = new WithdrawnEvent { Amount = amount, NameOf = "Withdraw", Version = CurrentVersion + 1};
            ApplyChange(@event);
            changes.Add(@event);
        }

        public void InitializeFromSnapshot(AccountSnapshot snapshot)
        {
            if (snapshot == null)
                throw new ArgumentNullException(nameof(snapshot));

            // اعمال تغییرات از طریق رویداد ساختگی
            var @event = new CreatedEvent
            {
                Id = snapshot.AccountId,
                Name = snapshot?.Name,
                initialBalance = snapshot.Balance,
                NameOf = "AccountSnapshot",
                Version = snapshot.CurrentVersion,
                Created = snapshot.Created,
                IsActive = snapshot.IsActive,

            };

            ApplyEventToState(@event);
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
            CurrentVersion = @event.Version;
        }


        private void ValidateEventVersion(EventBase @event)
        {
            if (@event is CreatedEvent && @event.Version != 1)
                throw new InvalidOperationException("CreatedEvent must have Version = 1");

            if (!(@event is CreatedEvent) && @event.Version != this.CurrentVersion + 1)
                throw new InvalidOperationException($"Event out of order. Expected: {this.CurrentVersion + 1}, Received: {@event.Version}");
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
                    CurrentVersion = createdEvent.Version;
                    break;
                case DepositedEvent depositedEvent:
                    CurrentVersion = depositedEvent.Version;
                    Balance += depositedEvent.Amount;
                    break;
                case WithdrawnEvent withdrawnEvent:
                    CurrentVersion = withdrawnEvent.Version;
                    Balance -= withdrawnEvent.Amount;
                    break;
            }
        }
    }
}
