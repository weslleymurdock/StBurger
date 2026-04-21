namespace StBurger.Domain.Core.Exceptions;

public class DependencyFailureException : DomainExceptionBase
{
    public DependencyFailureException(string dependencyName)
        : base($"Falha ao acessar a dependência '{dependencyName}'.", 424, "FAIL_DEPS") 
    {
    }
}
