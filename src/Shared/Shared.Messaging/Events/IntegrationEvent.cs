namespace Shared.Messaging.Events
{
    public record IntegrationEvent
    {
        public Guid EventId { get; }
        public DateTime OccurredOn { get; }
        public string EventType { get; }
    }
}