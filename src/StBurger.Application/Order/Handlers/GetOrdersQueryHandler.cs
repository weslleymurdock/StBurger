namespace StBurger.Application.Order.Handlers;

public sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, OrdersResponse>
{
    public Task<OrdersResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
