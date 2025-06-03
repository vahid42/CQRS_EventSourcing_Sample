using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AccountFlow.Api.V1.Entities
{
    public class Account
    {
        public Account() { }


        public Guid Id { get; protected set; }
        public decimal Balance { get; protected set; }
        public string Name { get; protected set; }
        public bool IsActive { get; protected set; }
        public DateTime Created { get; protected set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public Account(string name, decimal balance)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balance = balance;
            Created = DateTime.Now;
            IsActive = true;
            
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");
            Balance += amount;
            RowVersion = RowVersion;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
            RowVersion = RowVersion;
        }

    }
}
