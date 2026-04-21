namespace StBurger.Domain.Core.Exceptions;

public class ConflictException : DomainBaseException
{
    public ConflictException(string message)
        : base(message, 409, "CONFLICT") 
    {
    }
}
