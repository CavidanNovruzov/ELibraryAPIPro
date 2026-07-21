using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class WishlistItemConfiguration : BaseEntityConfiguration<WishlistItem>
{
    public override void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.WishlistId).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();

        builder.HasIndex(x => new { x.WishlistId, x.ProductId }).IsUnique();
        builder.HasIndex(x => x.ProductId);

        builder.HasOne(x => x.Wishlist)
            .WithMany(x => x.WishlistItems)
            .HasForeignKey(x => x.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.Product)
            .WithMany(x => x.WishlistItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
