namespace Application.Dtos.TrainingSessions;

public sealed record TrainingSessionDto
(
    Guid Id,
    string Name,
    DateTime StartTime,
    DateTime EndTime,
    int AvailableSpots,
    List<string> BookedIds
)
{
    public static TrainingSessionDto Create(
        Guid id,
        string name,
        DateTime startTime,
        DateTime endTime,
        int availableSpots,
        List<string> bookedIds
    )
    {
        return new(id, name, startTime, endTime, availableSpots, bookedIds);
    }
}