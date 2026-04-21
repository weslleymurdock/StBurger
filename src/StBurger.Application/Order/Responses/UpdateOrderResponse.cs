namespace StBurger.Application.Order.Responses;

public sealed record UpdateOrderResponse
    (string Id,
    string Attendant,
    string Customer,
    IReadOnlyCollection<MenuItemResponse> Items,
    decimal TotalPrice)
{
    internal static UpdateOrderResponse ToResponse(Domain.Orders.Entities.Order result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return new UpdateOrderResponse(
            Id: result.Id,
            Attendant: result.Attendant,
            Customer: result.Customer,
            Items: [.. result.Items.Select(item => MenuItemResponse.FromOrderItemResponse(item))],
            TotalPrice: result.Total
        );
    }
}