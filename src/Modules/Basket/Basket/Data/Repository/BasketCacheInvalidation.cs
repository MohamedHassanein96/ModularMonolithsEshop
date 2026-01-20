using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Data.Repository
{
    internal class BasketCacheInvalidation(IDistributedCache cache) : IBasketCacheInvalidation
    {
        public Task InvalidateAsync(string userName, CancellationToken cancellationToken = default)
        {
            return cache.RemoveAsync(userName, cancellationToken);  
        }

        public async Task InvalidateManyAsync(IEnumerable<string> userNames, CancellationToken cancellationToken = default)
        {
            foreach (var user in userNames)
                await cache.RemoveAsync(user, cancellationToken);
        }
    }
}
