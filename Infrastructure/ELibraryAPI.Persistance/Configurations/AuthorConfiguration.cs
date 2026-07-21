using ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class AuthorConfiguration : BaseEntityConfiguration<Author>
{
    public override void Configure(EntityTypeBuilder<Author> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Biography)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

        builder.Property(x => x.ImagePath)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.FullName);
        builder.HasIndex(x => x.Country);

        builder.HasMany(x => x.ProductAuthors)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
