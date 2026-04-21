namespace StBurger.Domain.Menu.Exceptions;

public class MenuItemNotFoundException : DomainBaseException
{
    public MenuItemNotFoundException(string itemName)
        : base($"Item do cardápio não encontrado: {itemName}", (int)System.Net.HttpStatusCode.NotFound, "MENU_ITEM_NOT_FOUND")
    {
    }
}
