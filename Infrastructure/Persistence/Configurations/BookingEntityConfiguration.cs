using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class BookingEntityConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => BookingId.Recreate(value)
            );

        builder.Property(x => x.TrainingSessionId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => TrainingSessionId.Recreate(value)
            );

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne(x => x.TrainingSession)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.TrainingSessionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}