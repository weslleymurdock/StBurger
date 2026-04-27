using StBurger.Domain.Menu.Enums;

namespace StBurger.Infrastructure.Persistence;

public class StBurgerDbContext(DbContextOptions<StBurgerDbContext> options) : AuditableContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Sandwich> Sandwiches { get; set; }
    public DbSet<Side> Sides { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<MenuItem> Catalog { get; set; }
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
            foreach (var entry in ChangeTracker.Entries<OrderItem>()
            .Where(e => e.State == EntityState.Added ||
            e.State == EntityState.Modified))
            {
                entry.Property("MenuItemType")
                    .CurrentValue = MapType(entry.Entity.MenuItem);
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private static MenuItemType MapType(MenuItem item) => item switch
    {
        Sandwich => MenuItemType.Sandwich,
        Side => MenuItemType.Side,
        Drink => MenuItemType.Drink,
        _ => throw new InvalidOperationException("Tipo desconhecido")
    };
}
