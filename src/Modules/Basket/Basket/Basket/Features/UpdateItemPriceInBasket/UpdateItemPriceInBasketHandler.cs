namespace Basket.Basket.Features.UpdateItemPriceInBasket

{
    public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price)
        :ICommand<UpdateItemPriceInBasketResult>;

    public record UpdateItemPriceInBasketResult(bool IsSuccess);

    public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
    {
        public UpdateItemPriceInBasketCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product is Rquired");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }



    internal class UpdateItemPriceInBasketHandler (BasketDbContext dbContext, IBasketCacheInvalidation basketCacheInvalidation)
        : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
    {
        // FIND SHOPPING CART WTH A GIVEN PRODUCT ID
        // ITERATE ITEMS AND UPDATE PRICE OF EVERY ITEM WITH INCOMING COMMAND-PRICE
        // SAVE TO DB
        // RETURN RESULT
        public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
        {


            var shoppingCartsToUpdateProductPrice = await dbContext.ShoppingCarts
                .Include(x=>x.Items)
                .Where(x=>x.Items.Any(i=>i.ProductId == command.ProductId))
                .ToListAsync(cancellationToken);


            if (!shoppingCartsToUpdateProductPrice.Any())
                return new UpdateItemPriceInBasketResult(false);
            


            foreach (var shoppingCart in shoppingCartsToUpdateProductPrice)
            {
                var item = shoppingCart.Items.Single(i => i.ProductId == command.ProductId);
                item.UpdatePrice(command.Price);
            };



            await dbContext.SaveChangesAsync(cancellationToken);


            await basketCacheInvalidation.InvalidateManyAsync(
                shoppingCartsToUpdateProductPrice.Select(x => x.UserName), cancellationToken);
                
          

            return new UpdateItemPriceInBasketResult(true);
        }
    }
}
