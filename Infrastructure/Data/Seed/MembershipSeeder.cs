using Application.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Membership;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Seed;

public class MembershipSeeder
{
    public static async Task SeedDefaultMemberships(IServiceProvider sp)
    {
        await using var scope = sp.CreateAsyncScope();
        var repo = scope.ServiceProvider.GetRequiredService<IMembershipRepository>();

        var memberships = new List<Membership>()
        {
            Membership.Create("Standard", 495),
            Membership.Create("Premium", 595),
        };

        foreach (var membership in memberships)
        {
            if (await repo.GetByName(membership.Name) is null)
                await repo.CreateAsync(membership);
        }
    }
}
