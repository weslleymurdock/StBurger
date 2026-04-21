namespace StBurger.Domain.Orders.Exceptions;

public class InvalidOrderException : DomainBaseException
{
    public InvalidOrderException(string reason)
        : base($"Invalid Order: {reason}", 422, "INVALID_ORDER")
    {
    }
}
