namespace StBurger.Domain.Core.Exceptions;

public class ConflictException : DomainExceptionBase
{
    public ConflictException(string message)
        : base(message, 409, "CONFLICT") 
    {
    }
}
