using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Orders.Features.CreateOrder;
using Shared.Messaging.Events;

namespace Ordering.Orders.EventHandlers
{
    public class BasketCheckoutIntegrationEventHandler
        (ISender sender, ILogger<BasketCheckoutIntegrationEventHandler> logger)
        : IConsumer<BasketCheckoutIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
        {
            logger.LogInformation(
                   "Domain Event Handled | Id={EventId} | Type={EventType} | OccuredOn={OccuredOn}",
                   context.Message.EventId,
                   context.Message.EventType,
                   context.Message.OccuredOn
            );


            var createOrderCommand = MapToCreateOrderCommand(context.Message);
            var result = await sender.Send(createOrderCommand);

            logger.LogInformation("OrderId == {OrderId}", result.OrderId);

        }
        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntegrationEvent message)
        {

            var orderItemsDto = message.OrderItemWriteDtos.Adapt<List<OrderItemWriteDto>>();

            var orderDto = new OrderWriteDto(
                CheckoutId: message.CheckoutId,
                CustomerId: message.CustomerId,
                OrderName: message.UserName,
                ShippingAddress: message.ShippingAddress,
                BillingAddress: message.BillingAddress,
                Payment: message.Payment,
                Items: orderItemsDto
            );


            return new CreateOrderCommand(orderDto);
        }
    }
}
