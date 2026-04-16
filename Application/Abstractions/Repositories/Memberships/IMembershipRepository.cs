using Domain.Aggregates.Membership.ValueObjects;
using Domain.Aggregates.Membership;

namespace Application.Abstractions.Repositories.Memberships;

public interface IMembershipRepository : IRepositoryBase<Membership, MembershipId>
{
    Task<Membership?> GetByName(string name, CancellationToken ct = default);
}
