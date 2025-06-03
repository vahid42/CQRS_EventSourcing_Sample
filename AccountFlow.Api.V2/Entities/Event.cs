namespace AccountFlow.Api.V2.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }
        public Guid AggregateId { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public int EventVersion { get; set; }

    }
}
