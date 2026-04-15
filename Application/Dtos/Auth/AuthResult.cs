namespace Application.Dtos.Auth;

public sealed record AuthResult
(
    bool Succeeded,
    AuthErrorType? ErrorType = null,
    string? ErrorMessage = null
)
{
    public static AuthResult Ok() => new(true);
    public static AuthResult Failed(string? errorMessage = null) => new(false, AuthErrorType.Error, errorMessage);
    public static AuthResult InvalidCredentials() => new(false, AuthErrorType.InvalidCredentials);
    public static AuthResult RequiresTwoFactorAuth() => new(false, AuthErrorType.RequiresTwoFactorAuth);
    public static AuthResult IsLockedOut() => new(false, AuthErrorType.IsLockedOut);
    public static AuthResult NotAllowed() => new(false, AuthErrorType.NotAllowed);
    public static AuthResult UserAlreadyExists(string? errorMessage = null) => new(false, AuthErrorType.UserAlreadyExists, errorMessage);
    public static AuthResult ExternalError(string? errorMessage = null) => new(false, AuthErrorType.ExternalError, errorMessage);
};
