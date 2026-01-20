namespace Ordering.Orders.Features.CreateOrder;

public record CreateOrderCommand(OrderWriteDto Order)
    :ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid OrderId);


public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is Required");
    }
}

internal class CreateOrderHandler(OrderingDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id);

    }

    private static Order CreateNewOrder (OrderWriteDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var payment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

        var newOrder = Order.Create(
            id: Guid.NewGuid(),  
            checkoutId: orderDto.CheckoutId,
            customerId: orderDto.CustomerId,
            orderName: $"{orderDto.OrderName}_{orderDto.CheckoutId}",
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: payment
            );

        orderDto.Items.ForEach(item =>
        {
            newOrder.AddItem(
               item.ProductId,
               item.Quantity,
               item.Price);
        });
        return newOrder;
    }
}
