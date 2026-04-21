namespace StBurger.Domain.Core.Exceptions;

public class ResourceGoneException : DomainExceptionBase
{
    public ResourceGoneException(string resourceName)
        : base($"O recurso '{resourceName}' não está mais disponível.", 410, "RESOURCE_GONE") 
    {
    }
}
