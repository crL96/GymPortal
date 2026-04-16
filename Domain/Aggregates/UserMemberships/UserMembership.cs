using Domain.Aggregates.Membership.ValueObjects;
using Domain.Common.Exceptions;

namespace Domain.Aggregates.UserMemberships;

public class UserMembership
{
    public string UserId { get; set; }
    public MembershipId MembershipId { get; set; }
    public bool IsActive { get; set; }

    private UserMembership(string userId, MembershipId membershipId, bool isActive)
    {
        UserId = userId;
        MembershipId = membershipId;
        IsActive = isActive;
    }

    public static UserMembership Create(string userId, MembershipId membershipId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidIdDomainException("UserId cannot be null or empty");

        return new(userId, membershipId, true);
    }

    public static UserMembership Recreate(string userId, MembershipId membershipId, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new InvalidIdDomainException("UserId cannot be null or empty");

        return new(userId, membershipId, isActive);
    }

    public void ChangePlan(MembershipId newMembershipId)
    {
        MembershipId = newMembershipId;
    }

    public void CancelPlan()
    {
        IsActive = false;
    }

    public void ReactivatePlan()
    {
        IsActive = true;
    }
}
