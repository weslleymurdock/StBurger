using System;
using System.Collections.Generic;
using System.Text;

namespace StBurger.Application.Order.Handlers;


public sealed class DeleteOrderCommandHandler(IOrderService service) : IRequestHandler<DeleteOrderCommand, Unit>
{
    public async Task<Unit> Handle
        (DeleteOrderCommand request, 
        CancellationToken cancellationToken) 
        => await service.DeleteOrderAsync(request.Id, cancellationToken);
}
