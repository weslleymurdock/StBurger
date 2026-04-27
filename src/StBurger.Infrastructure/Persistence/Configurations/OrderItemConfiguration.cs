using StBurger.Domain.Menu.Enums;

namespace StBurger.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Quantity)
               .IsRequired();

        builder.Property<MenuItemType>("MenuItemType");

        builder.HasIndex("OrderId", "MenuItemType")
               .IsUnique();

        builder.HasOne(oi => oi.MenuItem)
               .WithMany()
               .HasForeignKey("MenuItemId")
               .OnDelete(DeleteBehavior.Restrict);
    }
}
