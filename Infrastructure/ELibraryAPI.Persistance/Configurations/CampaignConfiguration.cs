using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class CampaignConfiguration : BaseEntityConfiguration<Campaign>
{
    public override void Configure(EntityTypeBuilder<Campaign> builder)
    {
        base.Configure(builder);

        builder.ToTable(x =>
        {
            x.HasCheckConstraint("CK_Campaigns_DiscountPercent_Range", "[DiscountPercent] >= 0 AND [DiscountPercent] <= 100");
        });

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.DiscountPercent)
            .HasDefaultValue(0)
            .HasPrecision(5, 2);

        builder.HasIndex(x => x.Title);
    }
}
