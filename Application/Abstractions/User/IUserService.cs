using Application.Dtos.User;

namespace Application.Abstractions.User;

public interface IUserService
{
    Task<UserResult> GetUserDetailsAsync(string userId);
    Task<UserResult> DeleteUserAsync(string userId);
    Task<UserResult> UpdateUserDetailsAsync(UserDetails details);
}
