using Application.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Aggregates.UserMembership;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Repositories.Memberships;

public class UserMembershipRepository(ApplicationDbContext context) :
    RepositoryBase<UserMembership, string, UserMembershipEntity, ApplicationDbContext>(context),
    IUserMembershipRepository
{
    protected override UserMembershipEntity ToEntity(UserMembership model)
    {
        return new UserMembershipEntity()
        {
            UserId = model.UserId,
            MembershipId = model.MembershipId.Value,
            IsActive = model.IsActive
        };
    }

    protected override UserMembership ToModel(UserMembershipEntity entity)
    {
        var membershipId = MembershipId.Recreate(entity.MembershipId);
        return UserMembership.Recreate(entity.UserId, membershipId, entity.IsActive);
    }
}
