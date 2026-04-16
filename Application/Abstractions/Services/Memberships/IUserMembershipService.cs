using Application.Dtos.Memberships;

namespace Application.Abstractions.Services.Memberships;

public interface IUserMembershipService
{
    Task<UserMembershipResult> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<UserMembershipResult> SignUpMembership(string userId, Guid membershipId, CancellationToken ct = default);
    Task<UserMembershipResult> ChangeMembership(string userId, Guid updatedMembershipId, CancellationToken ct = default);
    Task<UserMembershipResult> CancelMembership(string userId, CancellationToken ct = default);
    Task<UserMembershipResult> ReactivateMembership(string userId, CancellationToken ct = default);
}

