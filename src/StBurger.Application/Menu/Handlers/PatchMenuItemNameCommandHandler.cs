using StBurger.Application.Menu.Commands;

namespace StBurger.Application.Menu.Handlers;

public sealed class PatchMenuItemNameCommandHandler(IMenuService service) : IRequestHandler<PatchMenuItemNameCommand, Unit>
{
    public async Task<Unit> Handle(PatchMenuItemNameCommand request, CancellationToken cancellationToken) 
        => await service.PatchMenuItemNameAsync((id: request.Id, name: request.Name), cancellationToken);
}
