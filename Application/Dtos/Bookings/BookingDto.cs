namespace Application.Dtos.Bookings;

public sealed record BookingDto
(
    Guid Id,
    string UserId,
    TrainingSessionDetails TrainingSession
)
{
    public static BookingDto Create(
        Guid id,
        string userId,
        Guid sessionId,
        string sessionName,
        DateTime sessionStart,
        DateTime sessionEnd
    )
    {
        var sessionDetails = new TrainingSessionDetails(sessionId, sessionName, sessionStart, sessionEnd);
        return new(id, userId, sessionDetails);
    }
}

public sealed record TrainingSessionDetails
(
    Guid Id,
    string Name,
    DateTime StartTime,
    DateTime EndTime
);
