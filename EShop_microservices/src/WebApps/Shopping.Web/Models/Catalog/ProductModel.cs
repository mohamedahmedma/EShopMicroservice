namespace Shopping.Web.Models.Catalog;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public List<string> Category { get; set; } = new();
}

//wrapper classes
public record GetProductResponse(IEnumerable<ProductModel> products);
public record GetProductByCategoryResponse(IEnumerable<ProductModel> products);
public record GetProductByIdResponse(ProductModel product);