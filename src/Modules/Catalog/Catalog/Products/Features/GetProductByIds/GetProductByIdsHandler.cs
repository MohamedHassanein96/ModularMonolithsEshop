namespace Catalog.Products.Features.GetProductByIds;


internal class GetProductByIdsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByIdsQuery, GetProductsByIdsResult>
{
    public async Task<GetProductsByIdsResult> Handle(GetProductsByIdsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Where(p => query.ProductIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        var productsDto = products.Adapt<List<ProductDto>>();

        return new GetProductsByIdsResult(productsDto);
    }
}
