using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class PriceHistoryConfiguration : BaseEntityConfiguration<PriceHistory>
{
    public override void Configure(EntityTypeBuilder<PriceHistory> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {

            _.HasCheckConstraint("CK_PriceHistories_OldPrice_NonNegative", "[OldPrice] >= 0");
            _.HasCheckConstraint("CK_PriceHistories_NewPrice_NonNegative", "[NewPrice] >= 0");
        });

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.OldPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.NewPrice).HasPrecision(18, 2).IsRequired();

        builder.HasIndex(x => x.ProductId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.PriceHistories)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
