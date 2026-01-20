using Shared.Messaging.Dtos;

namespace Basket.Basket.Dtos;

public record BasketCheckoutDto
    (
    string UserName, 
    Guid CustomerId,
    AddressMessage BillingAddress,
    AddressMessage ShippingAddress,
    PaymentMessage Payment    
);
