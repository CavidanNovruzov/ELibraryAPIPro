using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class OrderConfiguration : BaseEntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {
            _.HasCheckConstraint("CK_Orders_TotalAmount_NonNegative", "[TotalAmount] >= 0");
        });

        builder.Property(x => x.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.OrderNote)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(x => x.OrderStatusId).IsRequired();
        builder.Property(x => x.PaymentMethodId).IsRequired();
        builder.Property(x => x.ShippingMethodId).IsRequired();

        builder.HasIndex(x => x.OrderNumber)
            .IsUnique();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.OrderStatusId);
        builder.HasIndex(x => x.PaymentMethodId);
        builder.HasIndex(x => x.ShippingMethodId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OrderStatus)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.OrderStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PaymentMethod)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ShippingMethod)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.ShippingMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Transactions)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

       
    }
}
