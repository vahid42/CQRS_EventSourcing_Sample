namespace AccountApi.CQRS
{
    public class WithdrawCommand
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }

    }
}
