using System.Collections.Immutable;

namespace StBurger.Application.Order.Responses;

public sealed record CreateOrderResponse
    (string Id,
    string Attendant,
    string Customer,
    List<MenuItemResponse> Items,
    decimal Discountered,
    decimal SubTotal,
    decimal TotalPrice)
{
    public static CreateOrderResponse ToResponse(Domain.Orders.Entities.Order result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return new CreateOrderResponse(
            Id: result.Id,
            Attendant: result.Attendant,
            Customer: result.Customer,
            Items: [.. result.Items.Select(item => MenuItemResponse.FromOrderItemResponse(item))],
            Discountered: result.Discount,
            SubTotal: result.Subtotal,
            TotalPrice: result.Total
        );
    }
}