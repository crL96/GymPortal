using Application.Abstractions.Repositories.Memberships;
using Application.Abstractions.Services.Memberships;
using Application.Dtos.Memberships;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Aggregates.UserMembership;
using Domain.Common.Exceptions;

namespace Application.Services;

public class UserMembershipService(IUserMembershipRepository userMembershipRepo, IMembershipRepository membershipRepo) : IUserMembershipService
{
    public async Task<UserMembershipResult> CancelMembership(string userId, CancellationToken ct = default)
    {
        try
        {
            var userMembership = await userMembershipRepo.GetByIdAsync(userId, ct);
            if (userMembership is null)
                return UserMembershipResult.NotFound();

            userMembership.CancelPlan();

            var success = await userMembershipRepo.UpdateAsync(userId, userMembership, ct);
            return success ?
                UserMembershipResult.Ok() :
                UserMembershipResult.Failed("Could not save to database");
        }
        catch (Exception ex)
        {
            return UserMembershipResult.Failed(ex.Message ?? "Could not update user membership");
        }
    }

    public async Task<UserMembershipResult> ChangeMembership(string userId, Guid updatedMembershipId, CancellationToken ct = default)
    {
        try
        {
            var userMembership = await userMembershipRepo.GetByIdAsync(userId, ct);
            if (userMembership is null)
                return UserMembershipResult.NotFound();

            userMembership.ChangePlan(MembershipId.Recreate(updatedMembershipId));

            var success = await userMembershipRepo.UpdateAsync(userId, userMembership, ct);
            return success ?
                UserMembershipResult.Ok() :
                UserMembershipResult.Failed("Could not save to database");
        }
        catch (Exception ex)
        {
            return UserMembershipResult.Failed(ex.Message ?? "Could not update user membership");
        }
    }

    public async Task<UserMembershipResult> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var userMembership = await userMembershipRepo.GetByIdAsync(userId, ct);
            if (userMembership is null)
                return UserMembershipResult.NotFound();

            var membershipDetails = await membershipRepo.GetByIdAsync(userMembership.MembershipId, ct);
            if (membershipDetails is null)
                return UserMembershipResult.NotFound();

            var dto = UserMembershipDto.Create(
                userMembership.UserId,
                userMembership.MembershipId.Value,
                membershipDetails.Name,
                userMembership.IsActive
            );
            return UserMembershipResult.Ok(dto);
        }
        catch (Exception ex)
        {
            return UserMembershipResult.Failed(ex.Message ?? "Couldn't fetch data from database");
        }
    }

    public async Task<UserMembershipResult> ReactivateMembership(string userId, CancellationToken ct = default)
    {
        try
        {
            var userMembership = await userMembershipRepo.GetByIdAsync(userId, ct);
            if (userMembership is null)
                return UserMembershipResult.NotFound();

            userMembership.ReactivatePlan();

            var success = await userMembershipRepo.UpdateAsync(userId, userMembership, ct);
            return success ?
                UserMembershipResult.Ok() :
                UserMembershipResult.Failed("Could not save to database");
        }
        catch (Exception ex)
        {
            return UserMembershipResult.Failed(ex.Message ?? "Could not update user membership");
        }
    }

    public async Task<UserMembershipResult> SignUpMembership(string userId, Guid membershipId, CancellationToken ct = default)
    {
        try
        {
            var exists = await userMembershipRepo.GetByIdAsync(userId, ct);
            if (exists is not null)
                return UserMembershipResult.AlreadyExists();

            var membershipIdDomain = MembershipId.Recreate(membershipId);
            var userMembership = UserMembership.Create(userId, membershipIdDomain);

            var created = await userMembershipRepo.CreateAsync(userMembership, ct);
            if (created is null)
                return UserMembershipResult.Failed("Could not save to database");

            return UserMembershipResult.Ok();
        }
        catch (InvalidIdDomainException ex)
        {
            return UserMembershipResult.InvalidId(ex.Message);
        }
        catch (Exception ex)
        {
            return UserMembershipResult.Failed(ex.Message);
        }
    }
}
