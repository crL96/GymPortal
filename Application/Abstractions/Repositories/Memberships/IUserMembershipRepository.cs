using Domain.Aggregates.UserMembership;

namespace Application.Abstractions.Repositories.Memberships;

public interface IUserMembershipRepository : IRepositoryBase<UserMembership, string>
{

}
