namespace StBurger.Infrastructure.Persistence.Configurations;

public class SandwichConfiguration : IEntityTypeConfiguration<Sandwich>
{
    public void Configure(EntityTypeBuilder<Sandwich> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();
    }
}
