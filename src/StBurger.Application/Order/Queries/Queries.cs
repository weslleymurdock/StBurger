namespace StBurger.Application.Order.Queries;

public sealed record GetOrderByIdQuery(string Id) : IRequest<OrderResponse>;
public sealed record GetOrdersQuery() : IRequest<OrdersResponse>;
