using Application.Abstractions.Auth;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class IdentityAuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : IAuthService
{
    public async Task<AuthResult> SignInUserAsync(string email, string password, bool rememberMe = false)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return AuthResult.InvalidCredentials();

        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);

        if (result.IsLockedOut)
            return AuthResult.IsLockedOut();

        if (result.IsNotAllowed)
            return AuthResult.NotAllowed();

        if (result.RequiresTwoFactor)
            return AuthResult.RequiresTwoFactorAuth();

        if (!result.Succeeded)
            return AuthResult.Failed();

        return AuthResult.Ok();
    }

    public async Task<AuthResult> SignUpUserAsync(string email, string password, string? roleName = null)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return AuthResult.InvalidCredentials();

        var user = await userManager.FindByEmailAsync(email);
        if (user is not null)
            return AuthResult.UserAlreadyExists();

        user = AppUser.Create(email);

        var created = await userManager.CreateAsync(user, password);
        if (!created.Succeeded)
            return AuthResult.Failed(
                created.Errors.FirstOrDefault()?.Description
                ?? "Failed to create user");

        if (!string.IsNullOrWhiteSpace(roleName) && await roleManager.RoleExistsAsync(roleName))
            await userManager.AddToRoleAsync(user, roleName);

        return AuthResult.Ok();
    }

    public Task SignOutUserAsync() => signInManager.SignOutAsync();
}
