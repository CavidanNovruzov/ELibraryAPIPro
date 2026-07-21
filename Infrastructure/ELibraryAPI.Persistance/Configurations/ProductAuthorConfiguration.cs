using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ProductAuthorConfiguration : BaseEntityConfiguration<ProductAuthor>
{
    public override void Configure(EntityTypeBuilder<ProductAuthor> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.AuthorId).IsRequired();

        builder.HasIndex(x => new { x.ProductId, x.AuthorId }).IsUnique();
        builder.HasIndex(x => x.AuthorId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductAuthors)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Author)
            .WithMany(x => x.ProductAuthors)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
