namespace StBurger.Application.Order.Handlers;

public sealed class DeleteOrderItemCommandHandler(IOrderService service) : IRequestHandler<DeleteOrderItemCommand, Unit>
{
    public async Task<Unit> Handle
        (DeleteOrderItemCommand request,
        CancellationToken cancellationToken) 
        => await service.DeleteItemAsync(request.OrderId, request.Id, cancellationToken);
}
