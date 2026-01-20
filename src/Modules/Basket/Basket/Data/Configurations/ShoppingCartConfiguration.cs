using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(sh => sh.Id);

        builder.HasIndex(sh => sh.UserName)
            .IsUnique();

        builder.Property(sh => sh.UserName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(sh => sh.Items)
            .WithOne()
            .HasForeignKey(i => i.ShoppingCartId);
    }
}

