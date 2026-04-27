using StBurger.Domain.Menu.Entities;

namespace StBurger.Application.Menu.Responses;

public sealed record UpdateMenuItemResponse(string Id, string Name, string Description, decimal Price)
{
    public static UpdateMenuItemResponse FromEntity(MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem, nameof(menuItem));
        return new UpdateMenuItemResponse(menuItem.Id, menuItem.Name, menuItem.Description, menuItem.Price);
    }
}