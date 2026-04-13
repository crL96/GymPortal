namespace Application.Dtos.User;

public sealed record UserResult(bool Succeeded, UserDetails? UserDetails = null, string? ErrorMessage = null)
{
    public static UserResult Ok(UserDetails? userDetails = null) => new(true, userDetails);
    public static UserResult Failed(string errorMessage) => new(false, null, errorMessage);
    public static UserResult NotFound(string errorMessage = "User not found") => new(false, null, errorMessage);
};
