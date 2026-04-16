using Application.Dtos.Memberships;

namespace Application.Abstractions.Services.Memberships;

public interface IMembershipService
{
    Task<MembershipResult> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<MembershipListResult> GetAllAsync(CancellationToken ct = default);
}
