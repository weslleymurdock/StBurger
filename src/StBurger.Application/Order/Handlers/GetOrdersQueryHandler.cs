using System.Collections.Immutable;

namespace StBurger.Application.Order.Handlers;

public sealed class GetOrdersQueryHandler(IOrderService service) : IRequestHandler<GetOrdersQuery, OrdersResponse>
{
    public async Task<OrdersResponse> Handle
        (GetOrdersQuery request, 
        CancellationToken cancellationToken) 
        => new([.. await service.GetAsync(cancellationToken)]);
}
