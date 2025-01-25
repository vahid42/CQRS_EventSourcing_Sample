namespace AccountApi.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public decimal Balance { get; private set; }
        public string Name { get; private set; }

        private Account() { }

        public Account(string name, decimal initialBalance)
        {
            Name = name;
            Id = Guid.NewGuid();
            Balance = initialBalance;
        }
    }
}
