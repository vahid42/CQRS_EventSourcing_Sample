namespace AccountApi.Events
{
    public class DepositedEvent : EventBase
    {
        public decimal Amount { get; set; }
    }
}
