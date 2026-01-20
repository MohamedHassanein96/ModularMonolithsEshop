namespace Catalog.Products.Events;

public record ProductPriceChangedEvent(Product Product) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName!;
}


