namespace AccountFlow.Api.V2.Events
{
    public class WithdrawnEvent : EventBase 
    {
        public decimal Amount { get; set; }
    }
}
