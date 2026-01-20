using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Orders;
using Ordering.Orders.ValueObjects;

namespace Ordering.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
       
        builder.HasKey(o => o.Id);

        builder.HasIndex(o => o.OrderName).IsUnique();

        builder.Property(o => o.OrderName)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(o => o.Items)
               .WithOne()
               .HasForeignKey(i => i.OrderId);

        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            address.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            address.Property(a => a.EmailAddress).HasMaxLength(50);
            address.Property(a => a.AddressLine).HasMaxLength(180).IsRequired();
            address.Property(a => a.Country).HasMaxLength(50);
            address.Property(a => a.State).HasMaxLength(50);
            address.Property(a => a.ZipCode).HasMaxLength(5).IsRequired();
        });

        builder.OwnsOne(o => o.BillingAddress, address =>
        {
            address.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
            address.Property(a => a.LastName).HasMaxLength(50).IsRequired();
            address.Property(a => a.EmailAddress).HasMaxLength(50);
            address.Property(a => a.AddressLine).HasMaxLength(180).IsRequired();
            address.Property(a => a.Country).HasMaxLength(50);
            address.Property(a => a.State).HasMaxLength(50);
            address.Property(a => a.ZipCode).HasMaxLength(5).IsRequired();
        });

        builder.OwnsOne(o => o.Payment, payment =>
        {
            payment.Property(p => p.CardName).HasMaxLength(50);
            payment.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
            payment.Property(p => p.Expiration).HasMaxLength(10);
            payment.Property(p => p.CVV).HasMaxLength(3);
            payment.Property(p => p.PaymentMethod).HasMaxLength(20);
        });
    }
}
