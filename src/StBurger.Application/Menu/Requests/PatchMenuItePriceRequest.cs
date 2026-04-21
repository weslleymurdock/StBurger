namespace StBurger.Application.Menu.Requests;

public sealed record PatchMenuItemPriceRequest(string Id, decimal Price);