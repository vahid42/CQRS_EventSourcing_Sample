namespace AccountApi.Events
{
    public class CreatedEvent : EventBase
    {
        public string Name { get; set; }
        public decimal initialBalance { get; set; }
    }
}
