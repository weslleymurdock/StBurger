namespace StBurger.Application.Order.Requests;

public sealed record CreateOrderRequest(string AttendantName, string CustomerName, IList<NewOrderItemRequest> Items)
{
    public static Domain.Orders.Entities.Order ToEntity(CreateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        return new Domain.Orders.Entities.Order(request.AttendantName, request.CustomerName);
    }
}