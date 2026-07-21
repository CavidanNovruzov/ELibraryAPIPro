using ELibraryAPI.Domain.Entities.Concrete.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations.Auth;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        // ── Audit sahələri (IdentityUserBaseEntity-dən gəlir) ──
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

        // ── Soft Delete sahələri ──
        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.DeletedAt)
            .HasColumnType("datetime2")
            .IsRequired(false);

        builder.Property(x => x.DeletedBy)
            .HasMaxLength(255)
            .IsRequired(false);

        // ── Əlavə sahələr ──
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        // FIX (Problem 8): Filtered indexes — low-cardinality boolean-larda daha effektiv.
        // Yalnız aktiv/silinməmiş istifadəçilər indeksləyir; optimizer bu indeksləri aktiv istifadə edir.
        builder.HasIndex(x => x.IsDeleted)
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(x => x.IsActive)
            .HasFilter("[IsActive] = 1");
    }
}
