namespace Application.Dtos.Auth;

public enum AuthErrorType
{
    InvalidCredentials,
    RequiresTwoFactorAuth,
    IsLockedOut,
    NotAllowed,
    UserAlreadyExists,
    ExternalError,
    Error
}
