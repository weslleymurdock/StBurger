namespace StBurger.Application.Menu.Queries;

public sealed record GetMenuQuery() : IRequest<MenuItemsResponse>;
public sealed record GetMenuItemQuery(string Id) : IRequest<MenuItemResponse>;

