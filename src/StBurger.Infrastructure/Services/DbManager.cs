using System;
using StBurger.Application.Core.Abstractions.Services;
using StBurger.Infrastructure.Persistence;
using StBurger.Infrastructure.Persistence.Utils;

namespace StBurger.Infrastructure.Services;

public class DbManager(StBurgerDbContext context, ILogger<DbManager> logger) : IDbManager
{
    public async Task MigrateAsync()
    {
        var pending = await context.Database.GetPendingMigrationsAsync();
        
        if (pending.Any())
        {
            logger.LogInformation("Applying pending migrations...");
            await context.Database.MigrateAsync();
        }
    }

    public async Task SeedAsync()
    {
        var pending = await context.Database.GetPendingMigrationsAsync();
        
        if (!pending.Any())
        {
            logger.LogInformation("No pending migrations. Seeding...");
            await DbSeeder.SeedAsync(context);
        }
    }
}
