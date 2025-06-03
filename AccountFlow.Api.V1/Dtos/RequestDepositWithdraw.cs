namespace AccountFlow.Api.V1.Dtos
{
    public class RequestDepositWithdraw
    {
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }

    }
}
