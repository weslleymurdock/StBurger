using StBurger.Domain.Menu.Exceptions;

namespace StBurger.Application.Menu.Handlers;

public class GetMenuItemQueryHandler(IMenuService service) 
    : IRequestHandler<GetMenuItemQuery, MenuItemResponse>
{
    public async Task<MenuItemResponse> Handle(GetMenuItemQuery request, CancellationToken cancellationToken) 
        => await service.GetAsync(request.Id, cancellationToken) ?? throw new MenuItemNotFoundException(request.Id);
}