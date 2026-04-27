using StBurger.Application.Menu.Commands;

namespace StBurger.Application.Menu.Handlers;

public sealed class PatchMenuItemPriceCommandHandler(IMenuService service) 
    : IRequestHandler<PatchMenuItemPriceCommand, Unit>
{
    public async Task<Unit> Handle
        (PatchMenuItemPriceCommand request, 
        CancellationToken cancellationToken) 
        => await service.PatchMenuItemPriceAsync(
            (id: request.Id, price: request.Price), 
            cancellationToken);
}
