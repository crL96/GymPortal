using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }

    public static AppUser Create(string email) => new()
    {
        UserName = email.Trim().ToLowerInvariant(),
        Email = email.Trim().ToLowerInvariant()
    };

    public static AppUser Create(string email, string? firstName, string? lastName, string? imageUrl) => new()
    {
        UserName = email.Trim().ToLowerInvariant(),
        Email = email.Trim().ToLowerInvariant(),
        FirstName = firstName?.Trim().ToLowerInvariant(),
        LastName = lastName?.Trim().ToLowerInvariant(),
        ImageUrl = imageUrl?.Trim()
    };
}
