namespace Shopping.Web.Pages;

public class ProductListModel(ICatalogService catalogService , IBasketService basketService , ILogger<ProductListModel> logger) 
    : PageModel
{
    public IEnumerable<string> CategoryList { get; set; } = [];
    public IEnumerable<ProductModel> ProductList { get; set; } = [];

    [BindProperty(SupportsGet =true)]
    public string SelectedCategory { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync(string CategoryName)
    {
        var response = await catalogService.GetProducts();

        CategoryList = response.products.SelectMany(p => p.Category).Distinct();

        if(! string.IsNullOrWhiteSpace(CategoryName))
        {
            ProductList = response.products.Where(p => p.Category.Contains(CategoryName));
            SelectedCategory = CategoryName;
        }
        else
        {
            ProductList = response.products;
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        logger.LogInformation("Add to cart button clicked");
        var productResponse = await catalogService.GetProduct(productId);

        var basket = await basketService.LoadUserBasket();

        basket.Items.Add(new ShoppingCartItemModel
        {
            ProductId = productId,
            ProductName = productResponse.product.Name,
            Price = productResponse.product.Price,
            Quantity = 1 ,
            Color = "Black"
        });

        await basketService.StoreBasket(new StoreBasketRequest(basket));

        return RedirectToPage("Cart");
    }
}
