namespace StBurger.Domain.Orders.Exceptions;

public class InvalidOrderException : DomainExceptionBase
{
    public InvalidOrderException(string reason)
        : base($"Invalid Order: {reason}", 422, "INVALID_ORDER")
    {
    }
}
