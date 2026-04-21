namespace StBurger.Domain.Orders.Exceptions;

public class OrderNotFoundException : DomainExceptionBase
{
    public OrderNotFoundException(string orderId)
        : base($"Order not found: {orderId}", (int)System.Net.HttpStatusCode.NotFound, "ORDER_NOT_FOUND")
    {
    }
}
