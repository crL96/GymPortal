using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class FaqEntityConfiguration : IEntityTypeConfiguration<FaqEntity>
{
    public void Configure(EntityTypeBuilder<FaqEntity> builder)
    {
        builder.ToTable("Faqs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired();
    }
}
