using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler
     (IBus bus,ILogger<ProductPriceChangedEventHandler> logger) 
    : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {

        logger.LogInformation(
          "Domain Event Handled | Id={EventId} | Type={EventType} | OccuredOn={OccuredOn}",
          notification.EventId,
          notification.EventType,
          notification.OccuredOn
         );



        //Async Commuincation
        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Category = notification.Product.Category,
            Description = notification.Product.Description,
            ImageFile = notification.Product.ImageFile,
            Price = notification.Product.Price,
         
        };

        await bus.Publish(integrationEvent, cancellationToken);
    }
}
