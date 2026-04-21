namespace StBurger.Application.Order.Commands;

public sealed record AddOrderItemCommand(UpdateMenuItemRequest Data) : IRequest<OrderResponse>;
public sealed record CreateOrderCommand(CreateOrderRequest Data) : IRequest<CreateOrderResponse>;
public sealed record DeleteOrderCommand(string id) : IRequest<Unit>;
public sealed record DeleteOrderItemCommand(string Id) : IRequest<Unit>;
public sealed record UpdateOrderCommand(UpdateOrderRequest Data) : IRequest<UpdateOrderResponse>;
