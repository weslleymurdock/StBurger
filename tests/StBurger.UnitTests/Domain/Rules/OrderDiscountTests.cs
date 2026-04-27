using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
using StBurger.UnitTests.Shared.Builders;

namespace StBurger.UnitTests.Domain.Rules;

public class OrderDiscountTests
{
    private readonly OrderBuilder builder = new();

    [Fact]
    public void Should_Apply_20Percent_Discount()
    {
        var order = builder.WithXBacon().WithCoke().WithFries().Build();

        Assert.Equal(11.5m, order.Subtotal);
        Assert.Equal(2.3m, order.Discount);
        Assert.Equal(9.2m, order.Total);
    }

    [Fact]
    public void Should_Apply_15Percent_Discount()
    {
        var order = builder.WithXEgg().WithCoke().Build();

        Assert.Equal(7, order.Subtotal);
        Assert.Equal(1.05m, order.Discount);
        Assert.Equal(5.95m, order.Total);
    }

    [Fact]
    public void Should_Apply_10Percent_Discount()
    {
        var order = builder.WithXBurger().WithFries().Build();

        Assert.Equal(7, order.Subtotal);
        Assert.Equal(0.7m, order.Discount);
        Assert.Equal(6.3m, order.Total);
    }

    [Fact]
    public void Should_Not_Apply_Discount()
    {
        var order = builder.WithXBacon().Build();
        Assert.Equal(7, order.Subtotal);
        Assert.Equal(0, order.Discount);
        Assert.Equal(7, order.Total);   
    }
}