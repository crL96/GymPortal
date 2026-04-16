namespace Application.Dtos.Memberships;

public sealed record UserMembershipResult(bool Succeeded, UserMembershipDto? UserMembership = null, string? ErrorMessage = null)
{
    public static UserMembershipResult Ok(UserMembershipDto? userMembership = null) => new(true, userMembership);
    public static UserMembershipResult Failed(string errorMessage) => new(false, null, errorMessage);
    public static UserMembershipResult AlreadyExists(string errorMessage = "User already has a membership") => new(false, null, errorMessage);
    public static UserMembershipResult InvalidId(string? errorMessage = null) => new(false, null, errorMessage);
    public static UserMembershipResult NotFound(string errorMessage = "Membership not found") => new(false, null, errorMessage);

}
