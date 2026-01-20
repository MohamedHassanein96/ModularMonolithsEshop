namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketRequest(string UserName, Guid ProductId);
public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}/items/{productId}",
              async ([FromRoute] string userName,
                     [FromRoute] Guid productId, 
                     ISender sender) =>
              {
                  var command = new RemoveItemFromBasketCommand(userName, productId);

                  await sender.Send(command);

                  return Results.NoContent();
              })
          
              .Produces(StatusCodes.Status204NoContent)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithSummary("Remove Item From Basket")
              .WithDescription("Remove Item From Basket");
    }
}
