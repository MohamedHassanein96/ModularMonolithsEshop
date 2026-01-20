using Catalog.Products.Features.GetProductByIds;

namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart)
    : ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart).NotNull();

        RuleFor(x => x.ShoppingCart.UserName).NotNull().NotEmpty().WithMessage("UserName is Required");

        RuleFor(x => x.ShoppingCart.Items).NotEmpty();

        RuleForEach(x => x.ShoppingCart.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).NotEmpty().WithMessage("ProductId is Required");
            item.RuleFor(i => i.Quantity).NotEmpty().GreaterThan(0).WithMessage("Must Be Greater than 0");
            item.RuleFor(i => i.Color).NotEmpty();
        });
    }
}

internal class CreateBasketHandler(IBasketRepository repository, ISender sender)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {

        var shoppingCart = CreateNewBasket(command.ShoppingCart);

        var productsIds = command.ShoppingCart.Items
            .Select(x => x.ProductId).Distinct().ToList();


        var query = new GetProductsByIdsQuery(productsIds);
        var result = await sender.Send(query, cancellationToken);


        var merged = from item in command.ShoppingCart.Items
                     join product in result.Products
                     on item.ProductId equals product.Id
                     into productGroup
                     from product in productGroup.DefaultIfEmpty()
                     select new
                     {
                         BasketItem = item,
                         Product = product
                     };


        foreach (var item in merged)
        {
            shoppingCart.AddItem(
                item.BasketItem.ProductId,
                item.BasketItem.Quantity,
                item.BasketItem.Color,
                item.Product.Price,
                item.Product.Name
                );
        }

        await repository.CreateBasket(shoppingCart, cancellationToken);

        return new CreateBasketResult(shoppingCart.Id);

    }

    private static ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCartDto)
    {
        var newBasket = ShoppingCart.Create(
            Guid.NewGuid(),
            shoppingCartDto.UserName);


        return newBasket;
    }

}
