using Application.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Membership;
using Domain.Aggregates.Membership.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Memberships;

public class MembershipRepository(ApplicationDbContext context) :
    RepositoryBase<Membership, MembershipId, MembershipEntity, ApplicationDbContext>(context),
    IMembershipRepository
{
    public async Task<Membership?> GetByName(string name, CancellationToken ct = default)
    {
        var entity = await Set.SingleOrDefaultAsync(x => x.Name == name, cancellationToken: ct);
        if (entity is null)
            return null;

        return ToModel(entity);
    }

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
