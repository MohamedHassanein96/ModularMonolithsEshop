namespace Basket.Basket.Features.DeleteItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
    :ICommand<Unit>;


public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}



internal class RemoveItemFromBasketHandler(IBasketRepository repository, IBasketCacheInvalidation basketCacheInvalidation)
    : ICommandHandler<RemoveItemFromBasketCommand>
{
    public async Task<Unit> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {

        var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);
        shoppingCart.RemoveItem(command.ProductId);


        await repository.SaveChangesAsync(cancellationToken);

        await basketCacheInvalidation.InvalidateAsync(command.UserName, cancellationToken);


        return Unit.Value;
    }
}
