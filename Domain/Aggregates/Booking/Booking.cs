using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Common.Exceptions;

namespace Domain.Aggregates.Booking;

public class Booking
{
    public BookingId Id { get; private set; }
    public string UserId { get; private set; }
    public TrainingSessionId TrainingSessionId { get; private set; }

    private Booking(BookingId id, string userId, TrainingSessionId trainingSessionId)
    {
        Id = id;
        UserId = userId;
        TrainingSessionId = trainingSessionId;
    }

    internal static Booking Create(string userId, TrainingSessionId trainingSessionId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidIdDomainException("UserId cannot be null or empty");

        var id = BookingId.Create();
        return new(id, userId, trainingSessionId);
    }

    public static Booking Recreate(BookingId id, string userId, TrainingSessionId trainingSessionId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidIdDomainException("UserId cannot be null or empty");

        return new(id, userId, trainingSessionId);
    }
}
