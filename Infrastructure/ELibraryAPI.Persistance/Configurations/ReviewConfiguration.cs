using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class ReviewConfiguration : BaseEntityConfiguration<Review>
{
    public override void Configure(EntityTypeBuilder<Review> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {
            _.HasCheckConstraint("CK_Reviews_Rating_Range", "[Rating] >= 1 AND [Rating] <= 5");
        });

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Comment)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.HasIndex(x => new { x.ProductId, x.UserId })
            .IsUnique();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.IsApproved);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
