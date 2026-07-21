using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ELibraryAPI.Domain.Entities.Concrete;

public sealed class BasketConfiguration : BaseEntityConfiguration<Basket>
{
    public override void Configure(EntityTypeBuilder<Basket> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasMany(x => x.BasketItems)
            .WithOne(x => x.Basket)
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithOne(x => x.Basket)
            .HasForeignKey<Basket>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}