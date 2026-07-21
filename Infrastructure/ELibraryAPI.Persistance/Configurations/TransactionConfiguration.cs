using global::ELibraryAPI.Domain.Entities.Concrete;
using ELibraryAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class TransactionConfiguration : BaseEntityConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);

        builder.ToTable(_ =>
        {
            _.HasCheckConstraint("CK_Transactions_Amount_NonNegative", "[Amount] >= 0");
        });

        builder.Property(x => x.OrderId).IsRequired();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.TransactionId)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(TransactionStatus.Pending);

        builder.Property(x => x.ProviderResponse)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(x => x.OrderId);
        builder.HasIndex(x => x.TransactionId)
            .IsUnique();

        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Order)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
