namespace AccountFlow.Api.V2.Events
{
    public class DepositedEvent : EventBase
    {
        public decimal Amount { get; set; }
    }
}
