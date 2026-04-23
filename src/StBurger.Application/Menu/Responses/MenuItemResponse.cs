using StBurger.Domain.Menu.Entities;

namespace StBurger.Application.Menu.Responses;

public record MenuItemResponse(string Id, string Name, decimal Price, string Description, string Type)
{
    internal static MenuItemResponse FromEntity(MenuItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.Id, item.Name, item.Price, item.Description, item switch
        {
            Drink _ => "Drink",
            Sandwich _ => "Sandwich",
            Side _ => "Side",
            _ => throw new NotSupportedException()
        });
    }

    internal static MenuItemResponse FromEntity(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.MenuItem.Id, item.MenuItem.Name, item.MenuItem.Price, item.MenuItem.Description, item.MenuItem switch
        {
            Drink _ => "Drink",
            Sandwich _ => "Sandwich",
            Side _ => "Side",
            _ => throw new NotSupportedException()
        });
    }

    internal static MenuItemResponse FromOrderItemResponse(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.MenuItem.Id, item.MenuItem.Name, item.MenuItem.Price, item.MenuItem.Description, item.MenuItem switch
        {
            Drink _ => "Drink",
            Sandwich _ => "Sandwich",
            Side _ => "Side",
            _ => throw new NotSupportedException()
        });
    }

    internal static MenuItemResponse ToResponse(MenuItem item)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        return new MenuItemResponse(item.Id, item.Name, item.Price, item.Description, item switch
        {
            Drink _ => "Drink",
            Sandwich _ => "Sandwich",
            Side _ => "Side",
            _ => throw new NotSupportedException()
        });
    }
}