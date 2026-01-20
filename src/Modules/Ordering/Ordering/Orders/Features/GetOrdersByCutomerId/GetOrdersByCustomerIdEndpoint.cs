using Ordering.Orders.Features.GetOrderById;

namespace Ordering.Orders.Features.GetOrdersByCutomerId;

public record GetOrdersByCustomerIdResponse(List<OrderReadDto> OrderReadDtos);

public class GetOrdersByCustomerIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/customers/{customerId}/orders", async (Guid customerId, ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersByCustomerIdQuery(customerId));
            var response = result.Adapt<GetOrdersByCustomerIdResponse>();

            return Results.Ok(response);
        })
         .WithName("GetOrderByCustomerId")
        .Produces<GetOrdersByCustomerIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Order By Customer Id")
        .WithDescription("Get Order Customer Id");
    }
}

