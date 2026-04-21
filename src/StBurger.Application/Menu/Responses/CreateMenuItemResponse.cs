namespace StBurger.Application.Menu.Responses;

public sealed record CreateMenuItemResponse(string Id, string Name, decimal Price, string Description);
