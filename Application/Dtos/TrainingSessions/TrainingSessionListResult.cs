namespace Application.Dtos.TrainingSessions;

public sealed record TrainingSessionListResult(bool Succeeded, List<TrainingSessionDto>? Sessions, string? ErrorMessage = null)
{
    public static TrainingSessionListResult Ok(List<TrainingSessionDto> sessions) => new(true, sessions);
    public static TrainingSessionListResult Failed(string errorMessage) => new(false, null, errorMessage);
}
