using ELibraryAPI.Domain.Entities.Concrete.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations.Auth;

public class AppUserPermissionConfiguration : IEntityTypeConfiguration<AppUserPermission>
{
    public void Configure(EntityTypeBuilder<AppUserPermission> builder)
    {
        // Primary Key (BaseEntity-dən gələn Id)
        builder.HasKey(x => x.Id);

        // 1. İcazəni alan istifadəçi (Recipient)
        builder.HasOne(x => x.User)
               .WithMany(u => u.UserPermissions)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade); // İstifadəçi silinəndə icazələri də silinsin

        // 2. İcazəni verən admin/istifadəçi (Grantor)
        builder.HasOne(x => x.GrantedByUser)
               .WithMany(u => u.GrantedPermissions)
               .HasForeignKey(x => x.GrantedByUserId)
               .IsRequired(false) // Nullable ola bilər (məsələn, sistem tərəfindən verilibsə)
               .OnDelete(DeleteBehavior.Restrict); // Admin silinəndə verdiyi icazələr silinməsin (Audit üçün)

        // 3. İcazənin növü (Permission entity-si ilə əlaqə)
        builder.HasOne(x => x.Permission)
               .WithMany() // Əgər Permission class-ında ICollection<AppUserPermission> yoxdursa boş saxla
               .HasForeignKey(x => x.PermissionId)
               .OnDelete(DeleteBehavior.Cascade);

        // Eyni istifadəçiyə eyni icazənin iki dəfə verilməməsi üçün Unique Index
        builder.HasIndex(x => new { x.UserId, x.PermissionId }).IsUnique();
    }
}