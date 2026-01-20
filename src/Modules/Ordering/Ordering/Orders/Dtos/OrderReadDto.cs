using Shared.Messaging.Dtos;

namespace Ordering.Orders.Dtos;

public record OrderReadDto(
    Guid Id,
    Guid CheckoutId,
    Guid CustomerId,
    string OrderName,
    AddressMessage ShippingAddress,
    AddressMessage BillingAddress,
    PaymentMessage Payment,
    List<OrderItemReadDto> Items);

