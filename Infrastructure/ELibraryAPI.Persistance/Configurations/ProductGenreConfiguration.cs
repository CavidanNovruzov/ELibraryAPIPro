using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ProductGenreConfiguration : BaseEntityConfiguration<ProductGenre>
{
    public override void Configure(EntityTypeBuilder<ProductGenre> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.GenreId).IsRequired();

        builder.HasIndex(x => new { x.ProductId, x.GenreId }).IsUnique();
        builder.HasIndex(x => x.GenreId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductGenres)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Genre)
            .WithMany(x => x.ProductGenres)
            .HasForeignKey(x => x.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
