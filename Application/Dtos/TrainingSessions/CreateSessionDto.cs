namespace Application.Dtos.TrainingSessions;

public sealed record CreateSessionDto
(
    string Name,
    DateTime StartTime,
    DateTime EndTime,
    int AvailableSpots
)
{
    public static CreateSessionDto Create(string name, DateTime StartTime, DateTime endTime, int availableSpots)
    {
        return new(name, StartTime, endTime, availableSpots);
    }
}