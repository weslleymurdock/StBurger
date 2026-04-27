namespace StBurger.Application.Order.Responses;

public sealed record OrderResponse
    (string Id,
    string Attendant,
    string Customer,
    IReadOnlyCollection<MenuItemResponse> Items,
    decimal Discountered,
    decimal SubTotal,
    decimal TotalPrice)
{
    public static OrderResponse FromEntity(Domain.Orders.Entities.Order e)
    {
        ArgumentNullException.ThrowIfNull(e);
        return new OrderResponse(
            Id: e.Id,
            Attendant: e.Attendant,
            Customer: e.Customer,
            Items: [.. e.Items.Select(MenuItemResponse.FromEntity)],
            Discountered: e.Discount,
            SubTotal: e.Subtotal,
            TotalPrice: e.Total
        );
    }
}