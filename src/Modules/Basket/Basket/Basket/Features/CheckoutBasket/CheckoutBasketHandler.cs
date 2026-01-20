using Shared.Messaging.Dtos;

namespace Basket.Basket.Features.CheckoutBasketHandler;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout)
    : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess, Guid CheckoutId);


public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
    }

}
internal class CheckoutBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{

    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        await using var transction =
            await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // Get existing basket with total price
            var basket = await dbContext.ShoppingCarts
                .Include(s => s.Items)
                .SingleOrDefaultAsync(s => s.UserName == command.BasketCheckout.UserName, cancellationToken);

            if (basket is null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }

            // Set totalprice on BasketCheckoutIntegrationEvent message
            // Set CheckoutId
            // map to OrderItemWriteDtos

            var orderItems = basket.Items.Adapt<List<OrderItemMessage>>();

            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>()
                with
            {
                CheckoutId = Guid.NewGuid(),
                TotalPrice = basket.TotalPrice,
                OrderItemWriteDtos = orderItems
            };


            // write the message to outbox table
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(eventMessage),
                OccuredOn = DateTime.UtcNow
            };

            // apply  outbox Pattern
            dbContext.OutboxMessages.Add(outboxMessage);


            // delete basket
            dbContext.ShoppingCarts.Remove(basket);


            await dbContext.SaveChangesAsync(cancellationToken);
            await transction.CommitAsync(cancellationToken);
            return new CheckoutBasketResult(true, eventMessage.CheckoutId);
        }

        catch (Exception)
        {
            await transction.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false, default);
        }
    }
}