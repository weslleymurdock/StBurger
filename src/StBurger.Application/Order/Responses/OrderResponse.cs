namespace StBurger.Application.Order.Responses;

public sealed record OrderResponse
    (string Id,
    string Customer,
    IReadOnlyCollection<MenuItemResponse> Items,
    decimal TotalPrice)
{
    internal static OrderResponse FromEntity(Domain.Orders.Entities.Order e)
    {
        ArgumentNullException.ThrowIfNull(e);
        return new OrderResponse(
            Id: e.Id,
            Customer: e.Customer,
            Items: [.. e.Items.Select(MenuItemResponse.FromEntity)],
            TotalPrice: e.Total
        );
    }
}