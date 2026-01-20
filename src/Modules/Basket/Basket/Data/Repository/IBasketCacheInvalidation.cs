namespace Basket.Data.Repository;

public interface IBasketCacheInvalidation
{
    Task InvalidateAsync(string userName, CancellationToken cancellationToken = default);
    Task InvalidateManyAsync(IEnumerable<string> userNames, CancellationToken cancellationToken = default);

}
