using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class BasketItemConfiguration : BaseEntityConfiguration<BasketItem>
{
    public override void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        base.Configure(builder);

        builder.ToTable(_=>
        {
            _.HasCheckConstraint("CK_BasketItems_Quantity_Positive", "[Quantity] > 0");
        });

        builder.Property(x => x.BasketId)
            .IsRequired();

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasDefaultValue(1)
            .IsRequired();

        builder.HasIndex(x => new { x.BasketId, x.ProductId })
            .IsUnique();

        builder.HasIndex(x => x.ProductId);

        builder.HasOne(x => x.Basket)
            .WithMany(x => x.BasketItems)
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.BasketItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
