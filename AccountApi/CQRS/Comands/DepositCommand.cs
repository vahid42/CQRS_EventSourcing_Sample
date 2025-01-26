namespace AccountApi.CQRS.Comands
{
    public class DepositCommand : ICommand<bool>
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
