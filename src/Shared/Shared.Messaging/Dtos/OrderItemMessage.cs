namespace Shared.Messaging.Dtos
{
    public record OrderItemMessage(Guid ProductId, int Quantity, decimal Price);

}
