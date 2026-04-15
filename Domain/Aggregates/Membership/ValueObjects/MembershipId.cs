using Domain.Common.Exceptions;

namespace Domain.Aggregates.Membership.ValueObjects;

public class MembershipId
{
    public Guid Value { get; private set; }

    public static MembershipId Create()
    {
        return new()
        {
            Value = Guid.NewGuid()
        };
    }

    public static MembershipId Recreate(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidIdDomainException("Id cannot be an empty GUID");

        return new()
        {
            Value = id
        };
    }
}
