namespace StBurger.Domain.Orders.Exceptions;

public class DuplicateItemException : DomainExceptionBase
{
    public DuplicateItemException(string itemName)
        : base($"Duplicated Item: {itemName}", (int)System.Net.HttpStatusCode.BadRequest, "DUPLICATE_ITEM") 
    {
    }
}
