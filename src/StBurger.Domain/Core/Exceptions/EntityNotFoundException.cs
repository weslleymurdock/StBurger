namespace StBurger.Domain.Core.Exceptions;

public sealed class EntityNotFoundException(string message, string key) : DomainExceptionBase(message, 404, key)
{
    
}
