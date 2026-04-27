using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
using StBurger.UnitTests.Shared.Factories;

namespace StBurger.UnitTests.Infrastructure.Persistence;

public class DbContextTests
{
    [Fact]
    public async Task Context_Should_Save_And_Load_Derived_MenuItem_Types()
    {
        using var context = TestDbContextFactory.Create();

        var sandwich = new Sandwich("X", "desc", 10);

        context.Catalog.Add(sandwich);

        using CancellationTokenSource c = new();
        await context.SaveChangesAsync(c.Token);

        var item = await context.Catalog.FirstAsync(c.Token);

        Assert.IsType<Sandwich>(item);
    }

    [Fact]
    public async Task Should_Save_And_Load_Derived_MenuItem_Types()
    {
        using var context = TestDbContextFactory.Create();

        var sandwich = new Sandwich("X", "desc", 10);

        context.Catalog.Add(sandwich);

        using CancellationTokenSource c = new();

        await context.SaveChangesAsync(c.Token);

        var item = await context.Catalog.FirstAsync(c.Token);

        Assert.IsType<Sandwich>(item);
    }

    [Fact]
    public async Task Should_Not_Delete_MenuItem_When_Referenced_By_OrderItem()
    {
        using var context = TestDbContextFactory.Create();

        var sandwich = new Sandwich("X", "desc", 10);
        context.Sandwiches.Add(sandwich);

        var order = new Order("att", "cust");
        order.AddItem(sandwich);
        using CancellationTokenSource c = new();

        context.Orders.Add(order);
        await context.SaveChangesAsync(c.Token);

        context.Sandwiches.Remove(sandwich);

        await Assert.ThrowsAsync<InvalidOperationException>(() => context.SaveChangesAsync(c.Token));
    }

    [Fact]
    public async Task Should_Set_MenuItemType_On_Save()
    {
        using var context = TestDbContextFactory.Create();

        var sandwich = new Sandwich("X", "desc", 10);
        context.Sandwiches.Add(sandwich);

        var order = new Order("att", "cust");
        order.AddItem(sandwich);

        context.Orders.Add(order);
        using CancellationTokenSource c = new();

        await context.SaveChangesAsync(c.Token);

        var entry = context.Entry(order.Items.First());

        var type = entry.Property("MenuItemType").CurrentValue;

        Assert.NotNull(type);
    }
}
