using Microsoft.EntityFrameworkCore.Internal;
using StBurger.Domain.Orders.Entities;
using StBurger.UnitTests.Shared.Factories;

namespace StBurger.UnitTests.Infrastructure.Persistence;

public class AuditableTests
{
    [Fact]
    public async Task SaveChanges_Should_Set_CreatedOn_On_Add()
    {
        using var context = TestDbContextFactory.Create();

        var order = new Order("att", "cust");
        
        using CancellationTokenSource c = new();
        
        context.Orders.Add(order);

        await context.SaveChangesAsync(c.Token);

        Assert.NotEqual(default, order.CreatedOn);
    }

    [Fact]
    public async Task SaveChanges_Should_Set_LastModifiedOn_On_Update()
    {
        using var context = TestDbContextFactory.Create();

        var order = new Order("att", "cust");
        
        using CancellationTokenSource c = new();
        
        context.Orders.Add(order);
        
        await context.SaveChangesAsync(c.Token);

        order.Customer = "updated";

        await context.SaveChangesAsync(c.Token);

        Assert.NotNull(order.LastModifiedOn);
    }
}
