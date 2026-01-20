namespace Ordering.Orders.Features.GetOrdersByCutomerId;

public record GetOrdersByCustomerIdQuery(Guid CustomerId)
    : IQuery<GetOrdersByCustomerIdResult>;

public record GetOrdersByCustomerIdResult(List<OrderReadDto> OrderReadDtos);

internal class GetOrdersByCustomerIdHandler(OrderingDbContext dbContext)
    : IQueryHandler<GetOrdersByCustomerIdQuery, GetOrdersByCustomerIdResult>
{
    public async Task<GetOrdersByCustomerIdResult> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(o => o.CustomerId == query.CustomerId)
            .ToListAsync(cancellationToken);

        var orderReadDtos = orders.Adapt<List<OrderReadDto>>();

        return new GetOrdersByCustomerIdResult(orderReadDtos);
    }
}
