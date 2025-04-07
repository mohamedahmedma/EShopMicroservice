
using CatalogAPI.Products.CreateProduct;

namespace CatalogAPI.Products.GetProductByCategory
{
    public record GetProductByCategoryRequest();
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}" 
                , async (ISender sender , string category) =>
            {
                var result = await sender.Send(new  GetProductByCategoryQuery(category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                //if (response is null)
                //{
                //    throw 
                //}
                return Results.Ok(response);
            })
            .WithName("GetProductByCategory")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");
        }
    }
}
