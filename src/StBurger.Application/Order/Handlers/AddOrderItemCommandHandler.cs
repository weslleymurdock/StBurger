namespace StBurger.Application.Order.Handlers;

public sealed class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, OrderResponse>
{
    public Task<OrderResponse> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
