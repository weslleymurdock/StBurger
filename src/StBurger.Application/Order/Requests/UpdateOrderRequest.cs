namespace StBurger.Application.Order.Requests;

public sealed record UpdateOrderRequest(string Id, string Attendant, string CustomerName, IReadOnlyCollection<NewOrderItemRequest> Items);
