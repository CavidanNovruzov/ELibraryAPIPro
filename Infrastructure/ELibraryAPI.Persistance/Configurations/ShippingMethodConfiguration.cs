using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ShippingMethodConfiguration : BaseEntityConfiguration<ShippingMethod>
{
    public override void Configure(EntityTypeBuilder<ShippingMethod> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {
            _.HasCheckConstraint("CK_ShippingMethods_Price_NonNegative", "[Price] >= 0");

        });

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.ShippingMethod)
            .HasForeignKey(x => x.ShippingMethodId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
