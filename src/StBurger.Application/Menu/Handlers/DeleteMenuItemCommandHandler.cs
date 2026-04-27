using StBurger.Application.Menu.Commands;

namespace StBurger.Application.Menu.Handlers;

public sealed class DeleteMenuItemCommandHandler(IMenuService service) 
    : IRequestHandler<DeleteMenuItemCommand, Unit>
{
    public async Task<Unit> Handle
        (DeleteMenuItemCommand request, 
        CancellationToken cancellationToken) 
        => await service.DeleteMenuItemAsync(request.Id, cancellationToken);
}
