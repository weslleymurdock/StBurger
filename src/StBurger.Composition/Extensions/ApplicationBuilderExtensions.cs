using System;
using Microsoft.AspNetCore.Builder;

namespace StBurger.Composition.Extensions;

public static class ApplicationBuilderExtensions
{
    extension(IApplicationBuilder app)
    {
        public async Task MigrateDatabaseAsync()
        {
            using var scope = app.ApplicationServices.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<IDbManager>();
            await migrator.MigrateAsync();
        }

        public async Task SeedDatabaseAsync()
        {
            using var scope = app.ApplicationServices.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<IDbManager>();
            await migrator.SeedAsync();
        }
    }
}
