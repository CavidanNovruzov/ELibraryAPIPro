using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {
            _.HasCheckConstraint("CK_Products_PageCount_Positive", "[PageCount] > 0");
            _.HasCheckConstraint("CK_Products_SalePrice_NonNegative", "[SalePrice] >= 0");
            _.HasCheckConstraint("CK_Products_DiscountPrice_Valid", "[DiscountPrice] IS NULL OR ([DiscountPrice] >= 0 AND [DiscountPrice] <= [SalePrice])");
        });

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.ISBN)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.PageCount)
            .IsRequired();

        builder.Property(x => x.SalePrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.DiscountPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.PublisherId).IsRequired();
        builder.Property(x => x.LanguageId).IsRequired();
        builder.Property(x => x.CoverTypeId).IsRequired();
        builder.Property(x => x.SubCategoryId).IsRequired();

        builder.HasIndex(x => x.ISBN)
            .IsUnique();

        builder.HasIndex(x => x.PublisherId);
        builder.HasIndex(x => x.LanguageId);
        builder.HasIndex(x => x.CoverTypeId);
        builder.HasIndex(x => x.SubCategoryId);

        builder.HasOne(x => x.Publisher)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Language)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CoverType)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CoverTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SubCategory)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Images)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Reviews)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ProductAuthors)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ProductGenres)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ProductTags)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.PriceHistories)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Stocks)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.InventoryMovements)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.BasketItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
