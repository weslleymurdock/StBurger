namespace StBurger.Application.Menu.Handlers;

public class GetMenuQueryHandler(IMenuService service) 
    : IRequestHandler<GetMenuQuery, MenuItemsResponse>
{
    public async Task<MenuItemsResponse> Handle(GetMenuQuery request, CancellationToken cancellationToken) 
        => new(await service.GetAsync(cancellationToken));
}