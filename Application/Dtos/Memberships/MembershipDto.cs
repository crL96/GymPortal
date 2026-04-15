namespace Application.Dtos.Memberships;

public sealed record MembershipDto
(
    Guid Id,
    string Name,
    int Price
)
{
    public static MembershipDto Create(Guid id, string name, int price)
    {
        return new(id, name, price);
    }
};
