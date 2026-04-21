namespace StBurger.Application.Menu.Requests;

public sealed record UpdateMenuItemRequest(string Id, string Name, string Description, decimal Price);
