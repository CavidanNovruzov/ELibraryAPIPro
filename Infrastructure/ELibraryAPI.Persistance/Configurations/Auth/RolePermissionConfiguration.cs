using ELibraryAPI.Domain.Entities.Concrete.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistance.Configurations.Auth;

public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
{
    public override void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // FIX (Problem 1): RolePermission artıq BaseEntity-dən irsiyyət alır (Id var).
        // base.Configure() → PK = Id. Composite PK silindi, natural key unique index ilə qorunur.
        base.Configure(builder);

        builder.ToTable("RolePermissions");

        // Natural key unique constraint — bir rola eyni icazənin təkrar verilməsini bloklayır
        builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
        builder.HasIndex(rp => rp.PermissionId);

        builder.HasOne(rp => rp.Role)
               .WithMany()
               .HasForeignKey(rp => rp.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
               .WithMany(p => p.RolePermissions)
               .HasForeignKey(rp => rp.PermissionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
