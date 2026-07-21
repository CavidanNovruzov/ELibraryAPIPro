using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ProductImageConfiguration : BaseEntityConfiguration<ProductImage>
{
    public override void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        base.Configure(builder);


        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.IsMain)
            .HasDefaultValue(false);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => new { x.ProductId, x.IsMain })
            .IsUnique()
            .HasFilter("[IsMain] = 1");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
