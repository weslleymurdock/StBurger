namespace StBurger.Application.Order.Handlers;

public sealed class CreateOrderCommandHandler(IOrderService service) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle
        (CreateOrderCommand request,
        CancellationToken cancellationToken) 
        => await service.CreateOrderAsync(request.Data, cancellationToken);
}
