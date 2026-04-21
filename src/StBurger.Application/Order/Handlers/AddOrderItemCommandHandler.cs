namespace StBurger.Application.Order.Handlers;

public sealed class AddOrderItemCommandHandler(IOrderService service) : IRequestHandler<AddOrderItemCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle
        (AddOrderItemCommand request,
        CancellationToken cancellationToken) =>
        await service.AddItemAsync(request.Id, request.Data.Id, cancellationToken);
}
