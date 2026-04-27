using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StBurger.Infrastructure.Persistence;

namespace StBurger.UnitTests.Shared.Factories;

public static class TestDbContextFactory
{
    public static StBurgerDbContext Create()
    {
        var options = new DbContextOptionsBuilder<StBurgerDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        var context = new StBurgerDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }
}