using System;
using System.Collections.Generic;
using System.Text;

namespace StBurger.Application.Order.Handlers;


public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
