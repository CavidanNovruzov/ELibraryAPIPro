using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class BranchConfiguration : BaseEntityConfiguration<Branch>
{
    public override void Configure(EntityTypeBuilder<Branch> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Location)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Phone)
            .HasMaxLength(30);

        builder.HasIndex(x => x.Phone).IsUnique();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasMany(x => x.WorkHours)
            .WithOne(x => x.Branch)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Stocks)
            .WithOne(x => x.Branch)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.OutgoingInventoryMovements)
            .WithOne(x => x.FromBranch)
            .HasForeignKey(x => x.FromBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.IncomingInventoryMovements)
            .WithOne(x => x.ToBranch)
            .HasForeignKey(x => x.ToBranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
