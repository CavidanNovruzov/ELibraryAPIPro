using global::ELibraryAPI.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELibraryAPI.Persistence.Configurations;

public sealed class UserSearchHistoryConfiguration : BaseEntityConfiguration<UserSearchHistory>
{
    public override void Configure(EntityTypeBuilder<UserSearchHistory> builder)
    {
        base.Configure(builder);


        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.SearchQuery)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.SearchHistories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
