namespace StBurger.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuração da entidade Order para o EF Core.
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Subtotal)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.Discount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.Total)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // Relacionamento 1:N entre Order e OrderItem
        builder.HasMany(o => o.Items)
               .WithOne()
               .HasForeignKey(i => i.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}


