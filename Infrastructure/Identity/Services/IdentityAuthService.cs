using System.Security.Claims;
using Application.Abstractions.Auth;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        if (await UserExists(email))
            return AuthResult.UserAlreadyExists();

        var user = AppUser.Create(email);

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

    public async Task<bool> UserExists(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        var exists = await userManager.Users.AnyAsync(x => x.Email == email);
        return exists;
    }

    public async Task<AuthResult> SignInExternalUserAsync(string? roleName = null)
    {
        var externalInfo = await signInManager.GetExternalLoginInfoAsync();
        if (externalInfo is null)
            return AuthResult.ExternalError();

        var email = externalInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
            return AuthResult.ExternalError($"No email address was provided by {externalInfo.LoginProvider}");

        var signInResult = await signInManager.ExternalLoginSignInAsync(
            externalInfo.LoginProvider,
            externalInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (signInResult.Succeeded)
            return AuthResult.Ok();

        var user = await userManager.FindByEmailAsync(email);
        if (user is not null)
            return AuthResult.UserAlreadyExists("Local account with same email already exists");

        var firstName = externalInfo.Principal.FindFirstValue(ClaimTypes.GivenName);
        var lastName = externalInfo.Principal.FindFirstValue(ClaimTypes.Surname);
        var imageUrl = externalInfo.LoginProvider switch
        {
            "GitHub" => externalInfo.Principal.FindFirstValue("urn:github:avatar"),
            _ => null
        };
        user = AppUser.Create(email, firstName, lastName, imageUrl);
        user.EmailConfirmed = true;

        var createdResult = await userManager.CreateAsync(user);
        if (!createdResult.Succeeded)
            return AuthResult.Failed(createdResult.Errors.FirstOrDefault()?.Description ?? "Failed to create user");

        if (!string.IsNullOrWhiteSpace(roleName))
        {
            var roleResult = await userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
                return AuthResult.Failed(createdResult.Errors.FirstOrDefault()?.Description ?? "Failed to add role to user");
        }

        var addLoginResult = await userManager.AddLoginAsync(user, externalInfo);
        if (!addLoginResult.Succeeded)
            return AuthResult.Failed(createdResult.Errors.FirstOrDefault()?.Description ?? "Failed to connect external login to user");

        await signInManager.SignInAsync(user, isPersistent: false);
        return AuthResult.Ok();
    }
}
