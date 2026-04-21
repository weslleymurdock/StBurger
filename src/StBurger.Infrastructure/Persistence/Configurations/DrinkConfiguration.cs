namespace StBurger.Infrastructure.Persistence.Configurations;

public class DrinkConfiguration : IEntityTypeConfiguration<Drink>
{
    public void Configure(EntityTypeBuilder<Drink> builder)
    { 

        builder.Property(d => d.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();
    }
}
