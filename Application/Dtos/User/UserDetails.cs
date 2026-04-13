namespace Application.Dtos.User;

public record class UserDetails
(
    string UserId,
    string Email,
    string? FirstName = null,
    string? LastName = null,
    string? PhoneNumber = null,
    string? ImageUrl = null
);