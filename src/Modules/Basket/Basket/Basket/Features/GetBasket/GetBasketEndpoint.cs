namespace Basket.Basket.Features.GetBasket;

public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCartDto ShoppingCart);
public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async ([AsParameters] GetBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<GetBasketQuery>();

            var result = await sender.Send(command);
            var response = result.Adapt<GetBasketResponse>();

            return Results.Ok(response);
        })
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Basket")
                .WithDescription("Get Basket");
    }
}
