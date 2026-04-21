namespace StBurger.Domain.Core.Exceptions;

public abstract class DomainBaseException : Exception
{
    public int Code { get; }
    public string Key { get; }
    protected DomainBaseException(string message, int code, string key) : base(message)
    {
        Code = code;
        Key = key;
    }
}

