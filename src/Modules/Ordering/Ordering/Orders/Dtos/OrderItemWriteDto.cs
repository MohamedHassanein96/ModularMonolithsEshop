namespace Ordering.Orders.Dtos;

public record OrderItemWriteDto(Guid ProductId, int Quantity, decimal Price);

