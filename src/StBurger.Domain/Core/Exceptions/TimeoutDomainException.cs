namespace StBurger.Domain.Core.Exceptions;

public class TimeoutDomainException : DomainBaseException
{
    public TimeoutDomainException(string operation)
        : base($"A operação '{operation}' excedeu o tempo limite.", 408, "TIMEOUT") // 408 Request Timeout
    {
    }
}
