using StBurger.Application.Menu.Commands;

namespace StBurger.Application.Menu.Handlers;

public sealed class UpdateMenuItemCommandHandler(IMenuService service) : IRequestHandler<UpdateMenuItemCommand, UpdateMenuItemResponse>
{
    public async Task<UpdateMenuItemResponse> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken) 
        => await service.UpdateMenuItemAsync(request.Data, cancellationToken);
}
