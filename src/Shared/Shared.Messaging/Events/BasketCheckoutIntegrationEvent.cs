using Shared.Messaging.Dtos;

namespace Shared.Messaging.Events;

public record BasketCheckoutIntegrationEvent :IntegrationEvent
{
    public string UserName { get; set; } = default!;
    public Guid CustomerId { get; set; } = default!;
    public decimal TotalPrice { get; set; } = default!;
    public AddressMessage BillingAddress {  get; set; } = default!;
    public AddressMessage ShippingAddress { get; set; } = default!;
    public PaymentMessage Payment { get; set; } = default!;
    public Guid CheckoutId { get; set; } = default!;    
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName!;
    public IReadOnlyList<OrderItemMessage> OrderItemWriteDtos { get; init; } = [];

}
