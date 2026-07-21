using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class PublisherConfiguration : BaseEntityConfiguration<Publisher>
{
    public override void Configure(EntityTypeBuilder<Publisher> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasMany(x => x.Products)
            .WithOne(x => x.Publisher)
            .HasForeignKey(x => x.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
