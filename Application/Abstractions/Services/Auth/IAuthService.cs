using Application.Dtos.Auth;

namespace Application.Abstractions.Services.Auth;

public interface IAuthService
{
    Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe = false);
    Task<AuthResult> SignUpUserAsync(string email, string password, string? roleName = null);
    Task<AuthResult> SignInExternalUserAsync(string? roleName = null);
    Task SignOutUserAsync();
    Task<bool> UserExists(string email);
}
