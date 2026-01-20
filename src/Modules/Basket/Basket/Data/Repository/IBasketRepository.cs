namespace Basket.Data.Repository;

public interface IBasketRepository
{
    Task<ShoppingCart>GetBasket(string userName,bool asNoTracking =true,CancellationToken cancellationToken = default);
    Task<ShoppingCart> CreateBasket(ShoppingCart cart,CancellationToken cancellationToken =default);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default); // bool review
    Task<int> SaveChangesAsync (CancellationToken cancellationToken = default); 
}
