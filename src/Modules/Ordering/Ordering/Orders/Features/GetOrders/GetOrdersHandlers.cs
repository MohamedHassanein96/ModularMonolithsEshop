namespace Ordering.Orders.Features.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest)
    :IQuery<GetOrdersResult>;

public record GetOrdersResult(PaginatedResult<OrderReadDto> Orders);


internal class GetOrdersHandlers(OrderingDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
       var pageSize = query.PaginationRequest.PageSize;
       var pageIndex = query.PaginationRequest.PageIndex;
       var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);


        var ordersDto = orders.Adapt <List<OrderReadDto>>();

        var ordersPaginatedResult = new PaginatedResult<OrderReadDto>(pageIndex, pageSize, totalCount, ordersDto);

        return new GetOrdersResult(ordersPaginatedResult);

    }
}