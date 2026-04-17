using Domain.Common.Exceptions;

namespace Domain.Aggregates.TrainingSessions.ValueObjects;

public sealed record TrainingSessionId
{
    public Guid Value { get; private set; }

    public static TrainingSessionId Create()
    {
        return new()
        {
            Value = Guid.NewGuid()
        };
    }

    public static TrainingSessionId Recreate(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidIdDomainException("Id cannot be an empty GUID");

        return new()
        {
            Value = id
        };
    }
}