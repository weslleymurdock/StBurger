using StBurger.Application.Menu.Commands;

namespace StBurger.Application.Menu.Handlers;

public sealed class PatchMenuItemDescriptionCommandHandler(IMenuService service) 
    : IRequestHandler<PatchMenuItemDescriptionCommand, Unit>
{
    public async Task<Unit> Handle
        (PatchMenuItemDescriptionCommand request, 
        CancellationToken cancellationToken) 
        => await service.PatchMenuItemDescriptionAsync(
            (id: request.Id, description: request.Description), 
            cancellationToken);
}
