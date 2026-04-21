using StBurger.Domain.Menu.Entities;

namespace StBurger.Application.Menu.Responses;

public record MenuItemResponse(string Name, decimal Price, string Description)
{
    internal static MenuItemResponse FromEntity(MenuItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.Name, item.Price, item.Description);
    }

    internal static MenuItemResponse FromEntity(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.MenuItem.Name, item.MenuItem.Price, item.MenuItem.Description);
    }

    internal static MenuItemResponse FromOrderItemResponse(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.MenuItem.Name, item.MenuItem.Price, item.MenuItem.Description);
    }

    internal static MenuItemResponse ToResponse(MenuItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.Name, item.Price, item.Description);
    }
}