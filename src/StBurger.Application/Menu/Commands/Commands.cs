namespace StBurger.Application.Menu.Commands;

public sealed record CreateMenuItemCommand(CreateMenuItemRequest Data) : IRequest<CreateMenuItemResponse>;
public sealed record DeleteMenuItemCommand(string Id) : IRequest<Unit>;
public sealed record PatchMenuItemDescriptionCommand(string Id, string Description) : IRequest<Unit>;
public sealed record PatchMenuItemNameCommand(string Id, string Name) : IRequest<Unit>;
public sealed record PatchMenuItemPriceCommand(string Id, decimal Price) : IRequest<Unit>;
public sealed record UpdateMenuItemCommand(UpdateMenuItemRequest Data) : IRequest<UpdateMenuItemResponse>;
