namespace Shared.Messaging.Events
{
    public record ProductPriceChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public List<string> Category { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; } = default!;


        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccuredOn { get; } = DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName!;
    }
}
