using StBurger.Domain.Core.Entities;
using StBurger.Domain.Menu.Entities;

namespace StBurger.Domain.Orders.Entities;
/// <summary>
/// Representa um item dentro de um pedido, referenciando um MenuItem específico.
/// </summary>
public class OrderItem : AuditableEntity<string>
{
    /// <summary>
    /// Identificador do pedido ao qual este item pertence.
    /// </summary>
    public string OrderId { get; private set; }

    /// <summary>
    /// Referência ao item do cardápio selecionado.
    /// </summary>
    public MenuItem MenuItem { get; private set; }

    /// <summary>
    /// Quantidade do item no pedido.
    /// </summary>
    public int Quantity { get; private set; }

    public OrderItem()
    {
        Id = Guid.NewGuid().ToString();
        Quantity = 0;
        OrderId = string.Empty;
        MenuItem = null!;
    }
    public OrderItem(string orderId, MenuItem menuItem, int quantity = 1)
    {
        Id = Guid.NewGuid().ToString();
        OrderId = orderId;
        MenuItem = menuItem;
        Quantity = quantity;
    }
}