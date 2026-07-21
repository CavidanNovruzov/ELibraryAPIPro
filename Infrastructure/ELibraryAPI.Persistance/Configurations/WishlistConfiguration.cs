using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class WishlistConfiguration : BaseEntityConfiguration<Wishlist>
{
    public override void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasMany(x => x.WishlistItems)
            .WithOne(x => x.Wishlist)
            .HasForeignKey(x => x.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Wishlist)
            .HasForeignKey<Wishlist>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
