using Domain.Aggregates.TrainingSessions.ValueObjects;

namespace Infrastructure.Persistence.Entities;

public class TrainingSessionEntity
{
    public TrainingSessionId Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AvailableSpots { get; set; }


    public List<BookingEntity> Bookings { get; set; } = [];
}
