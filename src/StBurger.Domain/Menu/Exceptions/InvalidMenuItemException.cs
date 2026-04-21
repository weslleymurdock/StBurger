namespace StBurger.Domain.Menu.Exceptions;

public class InvalidMenuItemException : DomainExceptionBase
{
    public InvalidMenuItemException(string reason)
        : base($"Item do cardápio inválido: {reason}", (int)System.Net.HttpStatusCode.UnprocessableEntity, "INVALID_MENU_ITEM")
    {
    }
}
