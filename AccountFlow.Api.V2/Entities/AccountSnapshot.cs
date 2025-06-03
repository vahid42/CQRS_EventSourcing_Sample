namespace AccountFlow.Api.V2.Entities
{
    public class AccountSnapshot
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Balance { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public int CurrentVersion { get; set; }
        public Guid RowVersion { get; set; }

    }
}
