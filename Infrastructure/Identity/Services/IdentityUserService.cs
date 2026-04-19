using Application.Abstractions.Services.User;
using Application.Dtos.User;
using Domain.Common.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class IdentityUserService(UserManager<AppUser> userManager) : IUserService
{
    public async Task<UserResult> GetUserDetailsAsync(string userId)
    {
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return UserResult.NotFound();

            if (user.Email is null)
                throw new MissingEmailDomainException();

            var userDetails = new UserDetails(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.ImageUrl
            );

            return UserResult.Ok(userDetails);
        }
    }

    public async Task<UserResult> UpdateUserDetailsAsync(UserDetails userDetails)
    {
        ArgumentNullException.ThrowIfNull(userDetails);

        var user = await userManager.FindByIdAsync(userDetails.UserId);
        if (user is null)
            return UserResult.NotFound();

        user.FirstName = userDetails.FirstName;
        user.LastName = userDetails.LastName;
        user.PhoneNumber = userDetails.PhoneNumber;
        user.ImageUrl = userDetails.ImageUrl;

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded
            ? UserResult.Ok()
            : UserResult.Failed(result.Errors.FirstOrDefault()?.Description ?? "Something went wrong, unable to save changes");
    }

    public async Task<UserResult> DeleteUserAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return UserResult.NotFound();

        var deleted = await userManager.DeleteAsync(user);
        return deleted.Succeeded
            ? UserResult.Ok()
            : UserResult.Failed(deleted.Errors.FirstOrDefault()?.Description ?? "Something went wrong, unable to delete account");
    }
}