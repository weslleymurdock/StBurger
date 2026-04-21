namespace StBurger.Application.Order.Handlers;

public sealed class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, Unit>
{
    public async Task<Unit> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
