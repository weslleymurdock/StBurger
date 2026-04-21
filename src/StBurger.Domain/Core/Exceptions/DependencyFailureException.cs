namespace StBurger.Domain.Core.Exceptions;

public class DependencyFailureException : DomainBaseException
{
    public DependencyFailureException(string dependencyName)
        : base($"Falha ao acessar a dependência '{dependencyName}'.", 424, "FAIL_DEPS") 
    {
    }
}
