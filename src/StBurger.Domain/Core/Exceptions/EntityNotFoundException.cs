namespace StBurger.Domain.Core.Exceptions;

public sealed class EntityNotFoundException(string message, string key) : DomainBaseException(message, 404, key)
{
    
}
