using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ProductTagConfiguration : BaseEntityConfiguration<ProductTag>
{
    public override void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.TagId).IsRequired();

        builder.HasIndex(x => new { x.ProductId, x.TagId }).IsUnique();
        builder.HasIndex(x => x.TagId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductTags)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tag)
            .WithMany(x => x.ProductTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
