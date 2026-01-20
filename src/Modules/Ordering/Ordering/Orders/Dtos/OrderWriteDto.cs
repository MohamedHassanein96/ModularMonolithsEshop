using Shared.Messaging.Dtos;

namespace Ordering.Orders.Dtos;

public record OrderWriteDto(
    Guid CheckoutId,
    Guid CustomerId,
    string OrderName,
    AddressMessage ShippingAddress,
    AddressMessage BillingAddress,
    PaymentMessage Payment,
    List<OrderItemWriteDto> Items);
