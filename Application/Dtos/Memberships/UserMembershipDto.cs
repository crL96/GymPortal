namespace Application.Dtos.Memberships;

public sealed record UserMembershipDto
(
    string UserId,
    Guid MembershipId,
    string MembershipName,
    bool IsActive
)
{
    public static UserMembershipDto Create(string userId, Guid membershipId, string membershipName, bool isActive)
    {
        return new(userId, membershipId, membershipName, isActive);
    }
};
