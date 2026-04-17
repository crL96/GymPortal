using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class ContactRequestEntityConfiguration : IEntityTypeConfiguration<ContactRequestEntity>
{
    public void Configure(EntityTypeBuilder<ContactRequestEntity> builder)
    {
        builder.ToTable("ContactRequests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.FirstName)
            .IsRequired();

        builder.Property(x => x.LastName)
            .IsRequired();

        builder.Property(x => x.Email)
            .IsRequired();

        builder.Property(x => x.Message)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
