using Domain.Aggregates.Membership.Exceptions;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Common.Exceptions;

namespace Domain.Aggregates.Membership;

public class Membership
{
    public MembershipId Id { get; private set; }
    public string Name { get; private set; }
    public int Price { get; private set; }

    private Membership(MembershipId id, string name, int price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public static Membership Create(string name, int price)
    {
        ValidateName(name);
        ValidatePrice(price);

        var id = MembershipId.Create();
        return new Membership(id, name.Trim().ToLowerInvariant(), price);
    }

    public static Membership Recreate(MembershipId id, string name, int price)
    {
        ValidateName(name);
        ValidatePrice(price);

        return new Membership(id, name, price);
    }

    public void Update(string? name = null, int? price = null)
    {
        if (name is not null)
        {
            ValidateName(name);
            Name = name.Trim();
        }

        if (price is not null && price > 0)
        {
            ValidatePrice((int)price);
            Price = (int)price;
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name.Trim()))
            throw new NullOrWhitespaceDomainException("Name can't be null or empty");
    }

    private static void ValidatePrice(int price)
    {
        if (price <= 0)
            throw new InvalidPriceDomainException("Price must be greater than 0");
    }
}
