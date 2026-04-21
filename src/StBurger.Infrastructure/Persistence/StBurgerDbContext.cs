namespace StBurger.Infrastructure.Persistence;

public class StBurgerDbContext(DbContextOptions<StBurgerDbContext> options, ILogger<AuditableContext> logger, ILogger<StBurgerDbContext> dbLogger) : AuditableContext(options, logger)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Sandwich> Sandwiches { get; set; }
    public DbSet<Side> Sides { get; set; }
    public DbSet<Drink> Drinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StBurgerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTimeOffset.UtcNow.DateTime;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTimeOffset.UtcNow.DateTime;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            dbLogger.LogWarning(e, "An error occurred: {Message}", e.Message);
            throw;
        }
    }

}
