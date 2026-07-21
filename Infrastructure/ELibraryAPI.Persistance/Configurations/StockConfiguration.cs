using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class StockConfiguration : BaseEntityConfiguration<Stock>
{
    public override void Configure(EntityTypeBuilder<Stock> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {
            _.HasCheckConstraint("CK_Stocks_Quantity_NonNegative", "[Quantity] >= 0");
        });

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.BranchId).IsRequired();
        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasIndex(x => new { x.ProductId, x.BranchId })
            .IsUnique();

        builder.HasIndex(x => x.BranchId);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Stocks)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Branch)
            .WithMany(x => x.Stocks)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
