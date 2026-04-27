using StBurger.Domain.Core.Entities;
using StBurger.Domain.Menu;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Exceptions;

namespace StBurger.Domain.Orders.Entities;

/// <summary>
/// Representa um pedido realizado na lanchonete, contendo itens do cardápio e regras de negócio aplicáveis.
/// </summary>
public class Order : AuditableEntity<string>
{
    /// <summary>
    /// Lista de itens do pedido, cada um referenciando um MenuItem.
    /// </summary>
    public List<OrderItem> Items { get; private set; } = [];

    /// <summary>
    /// Valor subtotal do pedido (soma dos preços dos itens).
    /// </summary>
    public decimal Subtotal { get; private set; }

    /// <summary>
    /// Valor do desconto aplicado ao pedido.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Valor total final do pedido após desconto.
    /// </summary>
    public decimal Total { get; private set; }
    public string Attendant { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;

    public Order(string attendant, string customer)
    {
        Id = Guid.NewGuid().ToString();
        Attendant = attendant;
        Customer = customer;
        Items = [];
        Subtotal = 0m;
        Discount = 0m;
        Total = 0m;
    }

    public Order()
    {
        Id = Guid.NewGuid().ToString();
        Items = [];
        Subtotal = 0m;
        Discount = 0m;
        Total = 0m;
    }


    /// <summary>
    /// Adiciona um item ao pedido, validando regras de negócio.
    /// </summary>
    /// <param name="menuItem">Item do cardápio a ser adicionado.</param>
    /// <exception cref="DuplicateItemException">Lançada quando já existe um item do mesmo tipo no pedido.</exception>
    public void AddItem(MenuItem menuItem)
    {
        if (Items.Any(i => i.MenuItem.GetType() == menuItem.GetType()))
            throw new DuplicateItemException(menuItem.Name);

        Items.Add(new OrderItem(Id, menuItem));
        CalculateTotals();
    }

    /// <summary>
    /// Remove um item do pedido.
    /// </summary>
    /// <param name="menuItemId">Identificador do item do cardápio.</param>
    public void RemoveItem(string menuItemId)
    {
        var item = Items.FirstOrDefault(i => i.MenuItem.Id == menuItemId);
        if (item != null)
        { 
            Items.Remove(item);
            CalculateTotals();
        }
    }

    /// <summary>
    /// Valida regras de negócio do pedido.
    /// </summary>
    /// <exception cref="InvalidOrderException">Lançada quando o pedido não contém ao menos um sanduíche.</exception>
    public void Validate()
    {
        if (!Items.Any(i => i.MenuItem is Sandwich))
            throw new InvalidOrderException("O pedido deve conter ao menos um sanduíche.");
    }

    /// <summary>
    /// Calcula e atualiza o subtotal, desconto e total do pedido com base nos itens selecionados e nas regras de desconto.
    /// </summary>
    private void CalculateTotals()
    {
        Subtotal = Items.Sum(i => i.MenuItem.Price * i.Quantity);
        Discount = CalculateDiscount();
        Total = Subtotal - Discount;
    }

    /// <summary>
    /// Calcula a quantia de desconto aplicável com base nos itens selecionados, seguindo as regras de desconto.
    /// </summary>
    /// <returns>Um valor decimal representando o desconto a ser aplicado no pedido.</returns>
    private decimal CalculateDiscount()
    {
        bool hasSandwich = Items.Any(i => i.MenuItem is Sandwich);
        bool hasSide = Items.Any(i => i.MenuItem is Side);
        bool hasDrink = Items.Any(i => i.MenuItem is Drink);

        if (hasSandwich && hasSide && hasDrink)
            return Subtotal * 0.20m;
        if (hasSandwich && hasDrink)
            return Subtotal * 0.15m;
        if (hasSandwich && hasSide)
            return Subtotal * 0.10m;

        return 0m;
    }

    public void Update(string attendant, string customerName, List<MenuItem> items)
    {
        Attendant = attendant;
        Customer = customerName;
        Items.Clear();
        foreach (var item in items)
        {
            AddItem(item);
        }
        Validate();
        CalculateTotals();
    }
}