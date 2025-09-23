
namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrderByNameHandler(IApplicationDbContext context)
    : IQueryHandler<GetOrderByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
    {
        var Orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(query.Name))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);


        return new GetOrdersByNameResult(Orders.ToOrderDtoList()); 
    }
}
