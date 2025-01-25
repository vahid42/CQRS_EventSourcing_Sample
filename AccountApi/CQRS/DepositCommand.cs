namespace AccountApi.CQRS
{
    public class DepositCommand
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
