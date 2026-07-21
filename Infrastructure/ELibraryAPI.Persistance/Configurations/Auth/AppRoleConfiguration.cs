using ELibraryAPI.Domain.Entities.Concrete.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations.Auth;

public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        builder.Property(x => x.UpdatedDate)
            .HasColumnType("datetime2")
            .IsRequired(false);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(255)
            .HasDefaultValue("System")
            .IsRequired();

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.DeletedAt)
            .HasColumnType("datetime2")
            .IsRequired(false);

        builder.Property(x => x.DeletedBy)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(x => x.IsDeleted);
    }
}
