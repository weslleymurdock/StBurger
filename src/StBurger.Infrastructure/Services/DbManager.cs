using System;
using StBurger.Application.Core.Abstractions.Services;
using StBurger.Infrastructure.Persistence;
using StBurger.Infrastructure.Persistence.Utils;

namespace StBurger.Infrastructure.Services;

public class DbManager(StBurgerDbContext context, ILogger<DbManager> logger) : IDbManager
{
    public async Task MigrateAsync()
    {
        try
        {
            var pending = await context.Database.GetPendingMigrationsAsync();

            if (pending.Any())
            {
                logger.LogInformation("Applying pending migrations...");
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "{Message}", e.Message);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            var pending = await context.Database.GetPendingMigrationsAsync();

            if (!pending.Any())
            {
                logger.LogInformation("No pending migrations. Seeding...");
                await DbSeeder.SeedAsync(context);
            }
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "{Message}", e.Message);
            throw;
        }
    }
}
