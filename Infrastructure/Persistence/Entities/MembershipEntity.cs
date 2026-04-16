using Domain.Aggregates.Membership.ValueObjects;

namespace Infrastructure.Persistence.Entities;

public class MembershipEntity
{
    public MembershipId Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Price { get; set; }

    public ICollection<UserMembershipEntity> UserMemberships { get; set; } = [];
}
