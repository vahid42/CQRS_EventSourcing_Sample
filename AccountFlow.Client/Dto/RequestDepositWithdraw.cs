namespace AccountFlow.Client.Dto
{
    public class RequestDepositWithdraw
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
