namespace AccountApi.CQRS.Comands
{
    public class WithdrawCommand : ICommand<bool>
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }

    }
}
