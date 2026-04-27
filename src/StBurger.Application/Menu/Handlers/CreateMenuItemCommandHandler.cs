using StBurger.Application.Menu.Commands;

namespace StBurger.Application.Menu.Handlers;

public sealed class CreateMenuItemCommandHandler(IMenuService service) 
    : IRequestHandler<CreateMenuItemCommand, CreateMenuItemResponse>
{
    public async Task<CreateMenuItemResponse> Handle
        (CreateMenuItemCommand request, 
        CancellationToken cancellationToken) 
        => await service.CreateMenuItemAsync(request.Data, cancellationToken);
}
