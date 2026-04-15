using Application.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Membership;
using Domain.Aggregates.Membership.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Repositories.Memberships;

public class MembershipRepository(ApplicationDbContext context) :
    RepositoryBase<Membership, MembershipId, MembershipEntity, ApplicationDbContext>(context),
    IMembershipRepository
{
    protected override MembershipEntity ToEntity(Membership model)
    {
        return new MembershipEntity()
        {
            Id = model.Id.Value,
            Name = model.Name,
            Price = model.Price
        };
    }

    protected override Membership ToModel(MembershipEntity entity)
    {
        var id = MembershipId.Recreate(entity.Id);
        return Membership.Recreate(id, entity.Name, entity.Price);
    }
}
