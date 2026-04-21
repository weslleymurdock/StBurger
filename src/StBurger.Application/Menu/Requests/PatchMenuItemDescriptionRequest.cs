namespace StBurger.Application.Menu.Requests;

public sealed record PatchMenuItemDescriptionRequest(string Id, string Description);