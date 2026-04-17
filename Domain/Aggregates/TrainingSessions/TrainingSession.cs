using Domain.Aggregates.TrainingSessions.Exceptions;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Common.Exceptions;

namespace Domain.Aggregates.TrainingSessions;

public class TrainingSession
{
    public TrainingSessionId Id { get; private set; }
    public string Name { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int AvailableSpots { get; private set; }

    private TrainingSession(TrainingSessionId id, string name, DateTime startTime, DateTime endTime, int availableSpots)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
        AvailableSpots = availableSpots;
    }

    public static TrainingSession Create(string name, DateTime startTime, DateTime endTime, int availableSpots)
    {
        ValidateName(name);
        ValidateTime(startTime, endTime);
        ValidateSpots(availableSpots);

        var id = TrainingSessionId.Create();
        return new TrainingSession(id, name.Trim(), startTime, endTime, availableSpots);
    }

    public static TrainingSession Recreate(TrainingSessionId id, string name, DateTime startTime, DateTime endTime, int availableSpots)
    {
        ValidateName(name);
        ValidateTime(startTime, endTime);
        ValidateSpots(availableSpots);

        return new TrainingSession(id, name.Trim(), startTime, endTime, availableSpots);
    }

    public bool IsFull(int numberOfBookings)
    {
        return numberOfBookings >= AvailableSpots;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name.Trim()))
            throw new NullOrWhitespaceDomainException("Name can't be null or empty");
    }

    private static void ValidateTime(DateTime startTime, DateTime endTime)
    {
        if (startTime < DateTime.Now)
            throw new InvalidTimeDomainException("Start time cannot be in the past");

        if (endTime < startTime.AddMinutes(30))
            throw new InvalidTimeDomainException("End time must be atleast 30 minutes after start time");
    }

    private static void ValidateSpots(int availableSpots)
    {
        if (availableSpots < 1)
            throw new NoAvailableSpotsDomainException();
    }
}
