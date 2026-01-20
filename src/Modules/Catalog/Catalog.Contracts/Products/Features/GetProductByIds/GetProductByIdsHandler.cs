namespace Catalog.Products.Features.GetProductByIds;

public record GetProductsByIdsQuery(IEnumerable<Guid> ProductIds)
    : IQuery<GetProductsByIdsResult>;

public record GetProductsByIdsResult(IEnumerable<ProductDto> Products);


