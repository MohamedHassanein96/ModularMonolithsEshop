namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketRequest(string UserName);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async ([AsParameters] DeleteBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteBasketRequest>();

            await sender.Send(new DeleteBasketCommand(request.UserName));


            return Results.NoContent();
        })
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Delete Basket")
                .WithDescription("Delete Basket");
    }
}
