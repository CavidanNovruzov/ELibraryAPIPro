using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class BannerConfiguration : BaseEntityConfiguration<Banner>
{
    public override void Configure(EntityTypeBuilder<Banner> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.RedirectUrl)
            .IsRequired(false)
            .HasMaxLength(500);
    }
}
