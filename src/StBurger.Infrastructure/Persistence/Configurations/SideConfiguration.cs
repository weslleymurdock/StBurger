namespace StBurger.Infrastructure.Persistence.Configurations;

public class SideConfiguration : IEntityTypeConfiguration<Side>
{
    public void Configure(EntityTypeBuilder<Side> builder)
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
