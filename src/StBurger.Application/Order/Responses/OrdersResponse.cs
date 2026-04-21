namespace StBurger.Application.Order.Responses;

public sealed record OrdersResponse(IReadOnlyCollection<OrderResponse> Orders);
