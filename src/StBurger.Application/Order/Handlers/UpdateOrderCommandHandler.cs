namespace StBurger.Application.Order.Handlers;

public sealed class UpdateOrderCommandHandler(IOrderService service) : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
{
    public async Task<UpdateOrderResponse> Handle
        (UpdateOrderCommand request, 
        CancellationToken cancellationToken) 
        => await service.UpdateOrderAsync(request.Data, cancellationToken);
}
