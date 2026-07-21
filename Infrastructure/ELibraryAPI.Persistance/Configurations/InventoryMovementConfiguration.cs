using ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class InventoryMovementConfiguration : BaseEntityConfiguration<InventoryMovement>
{
    public override void Configure(EntityTypeBuilder<InventoryMovement> builder)
    {
        base.Configure(builder);

        builder.ToTable(x =>
        {
            x.HasCheckConstraint("CK_InventoryMovements_Quantity_Positive", "[Quantity] > 0");

            x.HasCheckConstraint("CK_InventoryMovements_FromToBranchDifferent", "[ToBranchId] IS NULL OR [FromBranchId] <> [ToBranchId]");
        });

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.FromBranchId).IsRequired();

        builder.Property(x => x.ToBranchId).IsRequired(false);

        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(InventoryMovementStatus.Completed);

        builder.HasIndex(x => x.ProductId);
        builder.HasIndex(x => x.FromBranchId);
        builder.HasIndex(x => x.ToBranchId);
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.InventoryMovements)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FromBranch)
            .WithMany(x => x.OutgoingInventoryMovements)
            .HasForeignKey(x => x.FromBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToBranch)
            .WithMany(x => x.IncomingInventoryMovements)
            .HasForeignKey(x => x.ToBranchId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}