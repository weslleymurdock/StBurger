namespace StBurger.Domain.Core.Exceptions;

public abstract class DomainExceptionBase : Exception
{
    public int Code { get; }
    public string Key { get; }
    protected DomainExceptionBase(string message, int code, string key) : base(message)
    {
        Code = code;
        Key = key;
    }
}

