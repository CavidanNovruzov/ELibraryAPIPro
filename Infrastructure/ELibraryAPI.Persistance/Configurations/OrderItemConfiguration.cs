using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class OrderItemConfiguration : BaseEntityConfiguration<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ => {

            _.HasCheckConstraint("CK_OrderItems_Quantity_Positive", "[Quantity] > 0");
            _.HasCheckConstraint("CK_OrderItems_UnitPrice_NonNegative", "[UnitPrice] >= 0");
        });

        builder.Property(x => x.OrderId).IsRequired();
      
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.Quantity).HasDefaultValue(1).IsRequired();
        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);

        builder.HasIndex(x => new { x.OrderId, x.ProductId })
            .IsUnique();

        builder.HasIndex(x => x.ProductId);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
