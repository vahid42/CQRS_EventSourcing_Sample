namespace AccountFlow.Api.V2.Events
{
    public class CreatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal initialBalance { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
    }
}
