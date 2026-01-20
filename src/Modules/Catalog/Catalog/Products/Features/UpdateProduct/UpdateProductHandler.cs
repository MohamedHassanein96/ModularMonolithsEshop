namespace Catalog.Products.Features.UpdateProduct;


public record UpdateProductCommand(ProductDto Product)
  : ICommand;




public class UpdateProductCommandValidator :AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
};



internal class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand,Unit>
{
    public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FindAsync(command.Product.Id, cancellationToken);

        if (product == null)
        {
            throw new Exception();
        }

        UpdateProductsWithNewValues(product, command.Product);

        dbContext.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;

    }
    private static void UpdateProductsWithNewValues(Product product, ProductDto productDto)
    {
        product.Update(
              productDto.Name,
              productDto.Category,
              productDto.Description,
              productDto.ImageFile,
              productDto.Price
              );
    }
}
