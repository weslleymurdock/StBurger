using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
using StBurger.Domain.Orders.Exceptions;
using StBurger.UnitTests.Shared.Builders;

namespace StBurger.UnitTests.Domain.Entities;

public class OrderTests
{
    private readonly OrderBuilder builder = new();
    [Fact]
    public void Should_Add_Item_When_Valid()
    {
        var order = builder.WithXBacon().Build(); 
        Assert.Single(order.Items);
        Assert.Equal(7, order.Subtotal);
    }

    [Fact]
    public void Should_Not_Add_Duplicate_Item_Type()
    {
        var order = builder.WithXBacon().Build();
        var s2 = new Sandwich("B", "Sandwich", 15);
        Assert.Throws<DuplicateItemException>(() => order.AddItem(s2));
    }
}
