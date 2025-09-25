using Shopping.Web.Models.Ordering;

namespace Shopping.Web.Pages;

public class OrderListModel 
    (IOrderingService orderingService , ILogger<OrderListModel> logger)
    : PageModel
{
    public IEnumerable<OrderModel> Orders { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = new Guid("58C49479-EC65-4DE2-86E7-033C546291AA");

        var response = await orderingService.GetOrdersByCustomer(customerId);
        Orders = response.Orders;

        return Page();
    }
}
