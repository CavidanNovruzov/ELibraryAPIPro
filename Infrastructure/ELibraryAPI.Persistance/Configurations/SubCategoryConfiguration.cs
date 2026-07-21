using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class SubCategoryConfiguration : BaseEntityConfiguration<SubCategory>
{
    public override void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        base.Configure(builder);


        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CategoryId).IsRequired();

        builder.HasIndex(x => new { x.CategoryId, x.Name })
            .IsUnique();

        builder.HasIndex(x => x.CategoryId);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Products)
            .WithOne(x => x.SubCategory)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
