namespace StBurger.Application.Order.Handlers;

public sealed class GetOrderByIdQueryHandler(IOrderService service) : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<OrderResponse> Handle
        (GetOrderByIdQuery request, 
        CancellationToken cancellationToken) 
        => await service.GetAsync(request.Id, cancellationToken) 
        ?? throw new KeyNotFoundException($"Order with ID {request.Id} not found.");
}
