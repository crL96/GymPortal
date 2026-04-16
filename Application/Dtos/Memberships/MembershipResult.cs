namespace Application.Dtos.Memberships;

public sealed record MembershipResult(bool Succeeded, MembershipDto? Membership = null, string? ErrorMessage = null)
{
    public static MembershipResult Ok(MembershipDto? membership = null) => new(true, membership);
    public static MembershipResult Failed(string errorMessage) => new(false, null, errorMessage);
    public static MembershipResult NotFound(string errorMessage = "Membership not found") => new(false, null, errorMessage);

}
