using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Identity;

namespace Infrastructure.Persistence.Entities;

public class BookingEntity
{
    public BookingId Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public TrainingSessionId TrainingSessionId { get; set; } = null!;


    public AppUser User { get; set; } = null!;
    public TrainingSessionEntity TrainingSession { get; set; } = null!;
}
