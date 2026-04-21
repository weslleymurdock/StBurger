namespace StBurger.Application.Order.Commands;

public sealed record AddOrderItemCommand(string Id, NewOrderItemRequest Data) : IRequest<OrderResponse>;
public sealed record CreateOrderCommand(CreateOrderRequest Data) : IRequest<CreateOrderResponse>;
public sealed record DeleteOrderCommand(string Id) : IRequest<Unit>;
public sealed record DeleteOrderItemCommand(string OrderId, string Id) : IRequest<Unit>;
public sealed record UpdateOrderCommand(UpdateOrderRequest Data) : IRequest<UpdateOrderResponse>;
