namespace Application.Dtos.TrainingSessions;

public sealed record DeleteSessionResult(bool Succeeded, DeleteSessionErrorType? ErrorType = null, string? ErrorMessage = null)
{
    public static DeleteSessionResult Ok() => new(true);
    public static DeleteSessionResult Failed(string errorMessage) => new(false, DeleteSessionErrorType.Error, errorMessage);
    public static DeleteSessionResult InvalidId() => new(false, DeleteSessionErrorType.InvalidId);
    public static DeleteSessionResult Unauthorized() => new(false, DeleteSessionErrorType.Unauthorized);
}
