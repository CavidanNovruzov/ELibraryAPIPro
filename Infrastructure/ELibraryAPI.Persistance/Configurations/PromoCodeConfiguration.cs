using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class PromoCodeConfiguration : BaseEntityConfiguration<PromoCode>
{
    public override void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {
            _.HasCheckConstraint("CK_PromoCodes_DiscountPercent_Range", "[DiscountPercent] >= 0 AND [DiscountPercent] <= 100");
            _.HasCheckConstraint("CK_PromoCodes_DateRange", "[StartDate] < [EndDate]");
            _.HasCheckConstraint("CK_PromoCodes_UsageLimit_NonNegative", "[UsageLimit] >= 0");
        });

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.DiscountPercent)
            .HasPrecision(5, 2);

        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(x => x.UsageLimit).IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}
