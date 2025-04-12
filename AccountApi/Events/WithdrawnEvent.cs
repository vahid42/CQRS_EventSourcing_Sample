namespace AccountApi.Events
{
    public class WithdrawnEvent : EventBase 
    {
        public decimal Amount { get; set; }
    }
}
