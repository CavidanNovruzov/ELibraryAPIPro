using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class UserAddressConfiguration : BaseEntityConfiguration<UserAddress>
{
    public override void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        base.Configure(builder);


        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.AddressLine)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
