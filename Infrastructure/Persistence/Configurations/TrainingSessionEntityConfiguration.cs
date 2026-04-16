using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class TrainingSessionEntityConfiguration : IEntityTypeConfiguration<TrainingSessionEntity>
{
    public void Configure(EntityTypeBuilder<TrainingSessionEntity> builder)
    {
        builder.ToTable("TrainingSessions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => TrainingSessionId.Recreate(value)
            );

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.EndTime)
            .IsRequired();

        builder.Property(x => x.AvailableSpots)
            .IsRequired();
    }
}
