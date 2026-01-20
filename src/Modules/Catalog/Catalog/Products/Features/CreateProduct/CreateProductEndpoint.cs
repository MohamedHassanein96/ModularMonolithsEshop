namespace Catalog.Products.Features.CreateProduct;


public record CreateProductRequest(ProductDto Product);
public record CreateProductResponse(Guid Id);



public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async ([FromBody]CreateProductRequest request,[FromServices] ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/product/{response.Id}", response);
        })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("Create Product")
           .WithDescription("Create Product");
    }
}
