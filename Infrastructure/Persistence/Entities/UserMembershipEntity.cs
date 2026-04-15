using Infrastructure.Identity;

namespace Infrastructure.Persistence.Entities;

public class UserMembershipEntity
{
    public string UserId { get; set; } = null!;
    public Guid MembershipId { get; set; }
    public bool IsActive { get; set; }

    public MembershipEntity Membership { get; set; } = null!;
    public AppUser User { get; set; } = null!;
}
