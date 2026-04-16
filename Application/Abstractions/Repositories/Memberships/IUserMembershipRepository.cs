using Domain.Aggregates.UserMemberships;

namespace Application.Abstractions.Repositories.Memberships;

public interface IUserMembershipRepository : IRepositoryBase<UserMembership, string>
{

}
