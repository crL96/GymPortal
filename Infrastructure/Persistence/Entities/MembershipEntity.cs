namespace Infrastructure.Persistence.Entities;

public class MembershipEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Price { get; set; }

    public ICollection<UserMembershipEntity> UserMemberships { get; set; } = [];
}
