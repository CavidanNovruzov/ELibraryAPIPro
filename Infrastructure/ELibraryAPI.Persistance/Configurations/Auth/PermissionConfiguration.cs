using ELibraryAPI.Domain.Entities.Concrete.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistance.Configurations.Auth;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(p => p.Id);


        builder.Property(p => p.Key)
               .IsRequired()
               .HasMaxLength(100);

        // Authorization zamanı sürətli tapılması və təkrarın qarşısını almaq üçün
        builder.HasIndex(p => p.Key)
               .IsUnique();

        builder.Property(p => p.IsDelegatable)
               .HasDefaultValue(true);
    }
}
