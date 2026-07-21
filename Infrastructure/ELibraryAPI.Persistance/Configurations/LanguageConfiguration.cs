using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class LanguageConfiguration : BaseEntityConfiguration<Language>
{
    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(7);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasMany(x => x.Products)
            .WithOne(x => x.Language)
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
