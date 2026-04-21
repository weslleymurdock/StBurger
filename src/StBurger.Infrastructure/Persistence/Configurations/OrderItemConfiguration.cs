namespace StBurger.Infrastructure.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Quantity)
               .IsRequired();
         
        builder.HasOne(oi => oi.MenuItem)
               .WithMany()
               .HasForeignKey("MenuItemId")
               .OnDelete(DeleteBehavior.Restrict);
    }
}
