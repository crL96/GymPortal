namespace Application.Dtos.TrainingSessions;

public sealed record CreateSessionResult(bool Succeeded, string? ErrorMessage = null)
{
    public static CreateSessionResult Ok() => new(true);
    public static CreateSessionResult Failed(string errorMessage) => new(false, errorMessage);
}
