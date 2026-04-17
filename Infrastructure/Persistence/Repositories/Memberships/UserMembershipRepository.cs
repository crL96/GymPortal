using Application.Abstractions.Repositories.Memberships;
using Domain.Aggregates.UserMemberships;
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
            MembershipId = model.MembershipId,
            IsActive = model.IsActive
        };
    }

    protected override UserMembership ToModel(UserMembershipEntity entity)
    {
        return UserMembership.Recreate(entity.UserId, entity.MembershipId, entity.IsActive);
    }
}
