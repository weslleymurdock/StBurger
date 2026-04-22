using System;

namespace StBurger.Application.Core.Abstractions.Services;

public interface IDbManager
{
    Task MigrateAsync();

    Task SeedAsync();
}
