namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest  PaginationRequest)
    : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductDto> ProductDtos);

internal class GetProductsHandler(CatalogDbContext dbContext)
    : IRequestHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalCount = await dbContext.Products.CountAsync(cancellationToken);

        var products = await  dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var productsDtos = products.Adapt<List<ProductDto>>();

        var paginatedResult = new PaginatedResult<ProductDto>(pageIndex, pageSize, totalCount, productsDtos);

        return new GetProductsResult(paginatedResult);
            
    }
}
