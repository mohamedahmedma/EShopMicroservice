
using CatalogAPI.Products.CreateProduct;

namespace CatalogAPI.Products.GetProducts
{
    //public record GetProductRequest();
    public record GetProductsResponse(IEnumerable<Product> Products); // the same parameter in the Handler (result)
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery());

                var response = result.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces<CreateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get All Products"); 
        }
    }
}
