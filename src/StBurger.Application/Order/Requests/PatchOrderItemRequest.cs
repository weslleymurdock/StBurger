namespace StBurger.Application.Order.Requests;

public sealed record PatchOrderItemRequest(string Id, NewOrderItemRequest? OldItem, NewOrderItemRequest? NewItem);
