using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class UserMembershipEntityConfiguration : IEntityTypeConfiguration<UserMembershipEntity>
{
    public void Configure(EntityTypeBuilder<UserMembershipEntity> builder)
    {
        builder.ToTable("UserMemberships");

        builder.HasKey(x => x.UserId);

        builder.Property(x => x.MembershipId)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasOne(x => x.Membership)
            .WithMany(x => x.UserMemberships)
            .HasForeignKey(x => x.MembershipId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<UserMembershipEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}