using System.Collections.Immutable;

namespace StBurger.Application.Order.Responses;

public sealed record CreateOrderResponse
    (string Id,
    string Attendant,
    string Customer,
    List<MenuItemResponse> Items,
    decimal TotalPrice)
{
    internal static CreateOrderResponse ToResponse(Domain.Orders.Entities.Order result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return new CreateOrderResponse(
            Id: result.Id,
            Attendant: result.Attendant,
            Customer: result.Customer,
            Items: [.. result.Items.Select(item => MenuItemResponse.FromOrderItemResponse(item))],
            TotalPrice: result.Total
        );
    }
}