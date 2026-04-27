using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;

namespace StBurger.UnitTests.Shared.Builders;

public class OrderBuilder
{
    private readonly string attendant = "att";
    private readonly string customer = "cust";
    private List<MenuItem> items = [];
    
    public Order Build()
    {
        var order = new Order(attendant, customer);

        foreach (var item in items)
            order.AddItem(item);

        return order;
    }

    public OrderBuilder WithCoke()
    {
        items.Add(new Drink("Coke", "Drink", 2.5m));
        return this;
    }

    public OrderBuilder WithXBacon()
    {
        items.Add(new Sandwich("XBacon", "Sandwich", 7));
        return this;
    }

    public OrderBuilder WithXBurger()
    {
        items.Add(new Sandwich("XBurger", "Sandwich", 5));
        return this;
    }

    public OrderBuilder WithXEgg()
    {
        items.Add(new Sandwich("XEgg", "Sandwich", 4.5m));
        return this;
    }

    public OrderBuilder WithFries()
    {
        items.Add(new Side("Fries", "Side", 2));
        return this;
    }
}