namespace StBurger.Application.Menu.Requests;

public sealed record PatchMenuItemNameRequest(string Id, string Name);