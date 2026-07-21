using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ProductCampaignConfiguration : BaseEntityConfiguration<ProductCampaign>
{
    public override void Configure(EntityTypeBuilder<ProductCampaign> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.CampaignId).IsRequired(); 

        builder.HasIndex(x => new { x.ProductId, x.CampaignId }).IsUnique();
        builder.HasIndex(x => x.CampaignId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductCampaigns)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Campaign)
            .WithMany(x => x.ProductCampaigns)
            .HasForeignKey(x => x.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
