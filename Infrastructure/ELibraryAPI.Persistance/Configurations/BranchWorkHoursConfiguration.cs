using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class BranchWorkHoursConfiguration : BaseEntityConfiguration<BranchWorkHours>
{
    public override void Configure(EntityTypeBuilder<BranchWorkHours> builder)
    {
        base.Configure(builder);

        builder.ToTable(x=>
        {
            x.HasCheckConstraint("CK_BranchWorkHours_OpenBeforeClose", "[OpenTime] < [CloseTime]");
        });

        builder.Property(x => x.BranchId)
            .IsRequired();

        builder.Property(x => x.Day)
            .IsRequired();

        builder.Property(x => x.OpenTime)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.CloseTime)
            .HasColumnType("time")
            .IsRequired();

        builder.HasIndex(x => new { x.BranchId, x.Day })
            .IsUnique();

        builder.HasOne(x => x.Branch)
            .WithMany(x => x.WorkHours)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
