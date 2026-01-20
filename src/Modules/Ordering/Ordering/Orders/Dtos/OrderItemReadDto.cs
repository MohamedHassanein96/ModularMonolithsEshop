namespace Ordering.Orders.Dtos;

public record OrderItemReadDto(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal Price);

