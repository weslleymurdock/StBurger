using Microsoft.EntityFrameworkCore;
using StBurger.Domain.Core.Exceptions;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
using StBurger.Infrastructure.Persistence;
using StBurger.Infrastructure.Repositories;

namespace StBurger.UnitTests.Infrastructure.Repositories;

public class RepositoriesTests
{
    private static StBurgerDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<StBurgerDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        var context = new StBurgerDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }

    [Fact]
    public async Task AddAsync_Should_Add_Entity()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        var order = new Order("att", "cust");
        using CancellationTokenSource c = new();
        await repo.AddAsync(order);

        await context.SaveChangesAsync(c.Token);

        Assert.True(context.Orders.Any());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        var order = new Order("att", "cust");
        using CancellationTokenSource c = new();
        context.Orders.Add(order);
        await context.SaveChangesAsync(c.Token);

        var result = await repo.GetByIdAsync(order.Id);

        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_When_Not_Found()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            repo.GetByIdAsync(Guid.NewGuid().ToString()));
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        context.Orders.AddRange(
            new Order("a", "b"),
            new Order("c", "d"));
        using CancellationTokenSource c = new();
        await context.SaveChangesAsync(c.Token);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Entity()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        var order = new Order("att", "cust");
        using CancellationTokenSource c = new();
        context.Orders.Add(order);
        await context.SaveChangesAsync(c.Token);

        await repo.DeleteAsync(order);
        await context.SaveChangesAsync(c.Token);

        Assert.False(context.Orders.Any());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Entity()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        var order = new Order("att", "cust");
        using CancellationTokenSource c = new();
        context.Orders.Add(order);
        await context.SaveChangesAsync(c.Token);

        order.Customer = "updated";

        await repo.UpdateAsync(order);
        await context.SaveChangesAsync(c.Token);

        var updated = await context.Orders.FindAsync([order.Id], c.Token);

        Assert.Equal("updated", updated!.Customer);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Not_Found()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        var order = new Order("att", "cust");

        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            repo.UpdateAsync(order));
    }

    [Fact]
    public async Task GetPagedResponseAsync_Should_Return_Page()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        for (int i = 0; i < 10; i++)
            context.Orders.Add(new Order("a", "b"));
        using CancellationTokenSource c = new();
        await context.SaveChangesAsync(c.Token);

        var result = await repo.GetPagedResponseAsync(1, 5);

        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task GetPagedResponseAsync_Should_Throw_Invalid_Page()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            repo.GetPagedResponseAsync(0, 5));
    }

    [Fact]
    public async Task GetPagedResponseAsync_Should_Throw_When_Page_Exceeds()
    {
        using var context = CreateContext();
        var repo = new Repository<Order, string>(context);
        using CancellationTokenSource c = new();
        context.Orders.Add(new Order("a", "b"));
        await context.SaveChangesAsync(c.Token);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            repo.GetPagedResponseAsync(10, 5));
    }
    [Fact]
    public async Task GetByIdWithItemsAsync_Should_Return_Order_With_Items()
    {
        using var context = CreateContext();
        var uow = new UnitOfWork<string>(context);
        var repo = new OrderReadOnlyRepository(uow);

        var sandwich = new Sandwich("X", "desc", 10);
        context.Sandwiches.Add(sandwich);

        var order = new Order("att", "cust");
        order.AddItem(sandwich);
        using CancellationTokenSource c = new();

        context.Orders.Add(order);
        await context.SaveChangesAsync(c.Token);

        var result = await repo.GetByIdWithItemsAsync(order.Id, CancellationToken.None);

        Assert.NotEmpty(result.Items);
        Assert.NotNull(result.Items.First().MenuItem);
    }

    [Fact]
    public async Task GetByIdWithItemsAsync_Should_Throw_When_Not_Found()
    {
        using var context = CreateContext();
        var uow = new UnitOfWork<string>(context);
        var repo = new OrderReadOnlyRepository(uow);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            repo.GetByIdWithItemsAsync(Guid.NewGuid().ToString(), CancellationToken.None));
    }

    [Fact]
    public async Task GetWithItemsAsync_Should_Return_All_Orders_With_Items()
    {
        using var context = CreateContext();
        var uow = new UnitOfWork<string>(context);
        var repo = new OrderReadOnlyRepository(uow);

        var sandwich = new Sandwich("X", "desc", 10);
        context.Sandwiches.Add(sandwich);

        var order = new Order("att", "cust");
        order.AddItem(sandwich);
        using CancellationTokenSource c = new();

        context.Orders.Add(order);
        await context.SaveChangesAsync(c.Token);

        var result = await repo.GetWithItemsAsync(CancellationToken.None);

        Assert.Single(result);
        Assert.NotEmpty(result.First().Items);
    }
}