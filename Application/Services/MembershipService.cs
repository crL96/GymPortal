using Application.Abstractions.Repositories.Memberships;
using Application.Abstractions.Services.Memberships;
using Application.Dtos.Memberships;
using Domain.Aggregates.Membership.ValueObjects;

namespace Application.Services;

public class MembershipService(IMembershipRepository repo) : IMembershipService
{
    public async Task<MembershipListResult> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var memberships = await repo.GetAllAsync(ct);
            var dtos = memberships.Select(x => MembershipDto.Create(x.Id.Value, x.Name, x.Price)).ToList();

            return MembershipListResult.Ok(dtos);
        }
        catch (Exception ex)
        {
            return MembershipListResult.Failed(ex.Message ?? "Couldn't fetch data from database");
        }
    }

    public async Task<MembershipResult> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var membership = await repo.GetByIdAsync(MembershipId.Recreate(id), ct);
            if (membership is null)
                return MembershipResult.NotFound();

            var dto = MembershipDto.Create(membership.Id.Value, membership.Name, membership.Price);
            return MembershipResult.Ok(dto);
        }
        catch (Exception ex)
        {
            return MembershipResult.Failed(ex.Message ?? "Couldn't fetch data from database");
        }
    }
}
