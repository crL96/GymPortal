namespace Application.Dtos.Memberships;

public sealed record MembershipListResult(bool Succeeded, IReadOnlyList<MembershipDto>? Memberships = null, string? ErrorMessage = null)
{
    public static MembershipListResult Ok(IReadOnlyList<MembershipDto> memberships) => new(true, memberships);
    public static MembershipListResult Failed(string errorMessage) => new(false, null, errorMessage);
}