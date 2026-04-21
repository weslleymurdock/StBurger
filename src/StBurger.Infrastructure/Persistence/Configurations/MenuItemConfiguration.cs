namespace StBurger.Infrastructure.Persistence.Configurations;

public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasMaxLength(200);

        builder.Property(m => m.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // Discriminator para mapear os tipos derivados
        builder.HasDiscriminator<string>("MenuItemType")
               .HasValue<Sandwich>("Sandwich")
               .HasValue<Side>("Side")
               .HasValue<Drink>("Drink");
    }
}

