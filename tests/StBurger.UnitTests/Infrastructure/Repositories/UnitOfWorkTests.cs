using Microsoft.EntityFrameworkCore;
using StBurger.Domain.Menu.Entities;
using StBurger.Domain.Orders.Entities;
using StBurger.Infrastructure.Persistence;
using StBurger.Infrastructure.Repositories;

namespace StBurger.UnitTests.Infrastructure.Repositories;

public class UnitOfWorkTests
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
    public void Repository_Should_Return_Same_Instance_For_Same_Type()
    {
        using var context = CreateContext();
        var uow = new UnitOfWork<string>(context);

        var repo1 = uow.Repository<Order>();
        var repo2 = uow.Repository<Order>();

        Assert.Same(repo1, repo2);
    }

    [Fact]
    public void Repository_Should_Return_Different_Instance_For_Different_Types()
    {
        using var context = CreateContext();
        var uow = new UnitOfWork<string>(context);

        var repo1 = uow.Repository<Order>();
        var repo2 = uow.Repository<MenuItem>();

        Assert.NotSame(repo1, repo2);
    }

    [Fact]
    public async Task Commit_Should_Persist_Data()
    {
        using var context = CreateContext();
        var uow = new UnitOfWork<string>(context);

        var repo = uow.Repository<Order>();

        var order = new Order("att", "cust");

        await repo.AddAsync(order);
        await uow.Commit(CancellationToken.None);

        var exists = context.Orders.Any(x => x.Id == order.Id);

        Assert.True(exists);
    }

    [Fact]
    public async Task Rollback_Should_Revert_Changes()
    {
        using var context = CreateContext();
        var uow = new UnitOfWork<string>(context);

        var order = new Order("att", "cust");

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        order.Customer = "changed";

        await uow.Rollback();

        var dbOrder = await context.Orders.FindAsync(order.Id);

        Assert.Equal("cust", dbOrder!.Customer);
    }

    [Fact]
    public void Dispose_Should_Dispose_Context()
    {
        var context = CreateContext();
        var uow = new UnitOfWork<string>(context);

        uow.Dispose();

        Assert.Throws<ObjectDisposedException>(() => context.Orders.ToList());
    }
}