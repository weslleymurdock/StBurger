namespace StBurger.Application.Menu.Requests;

public sealed record CreateMenuItemRequest(string Name, decimal Price, string Description, string Type);
