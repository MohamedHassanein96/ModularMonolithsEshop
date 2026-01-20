namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string UserName)
    :ICommand<Unit>;

internal class DeleteBasketHandler(IBasketRepository repository)
    : ICommandHandler<DeleteBasketCommand>
{
    public async Task<Unit> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {

        await repository.DeleteBasket(command.UserName, cancellationToken);
        return Unit.Value;
    }
}
