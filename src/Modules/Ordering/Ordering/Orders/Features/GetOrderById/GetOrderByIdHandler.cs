namespace Ordering.Orders.Features.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId)
    : ICommand<GetOrderByIdResult>;

public record GetOrderByIdResult(OrderReadDto Order);

internal class GetOrderByIdHandler(OrderingDbContext dbContext) 
    : ICommandHandler<GetOrderByIdQuery, GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .Where(o => o.Id == query.OrderId)
            .FirstOrDefaultAsync(cancellationToken);


        if (order is null)
        {
            throw new OrderNotFoundException(query.OrderId);
        }

        var orderDto = order.Adapt<OrderReadDto>();

        return new GetOrderByIdResult(orderDto);
    }
}
