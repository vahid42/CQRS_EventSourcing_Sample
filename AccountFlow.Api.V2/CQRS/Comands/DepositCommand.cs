namespace AccountFlow.Api.V2.CQRS.Comands
{
    public class DepositCommand : ICommand<bool>
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
