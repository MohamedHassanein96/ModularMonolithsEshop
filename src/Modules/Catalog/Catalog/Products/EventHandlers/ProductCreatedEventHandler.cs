namespace Catalog.Products.EventHandlers;

public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain Event Handled | Id={EventId} | Type={EventType} | OccuredOn={OccuredOn}",
            notification.EventId,
            notification.EventType,
            notification.OccuredOn
        );
        return Task.CompletedTask;
    }
}
