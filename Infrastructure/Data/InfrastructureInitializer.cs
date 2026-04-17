using Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data;

public static class InfrastructureInitializer
{
    public static async Task InitializeAsync(IServiceProvider sp, IConfiguration configuration, IWebHostEnvironment env)
    {
        await PersistenceInitializer.InitializeDatabaseAsync(sp, configuration, env);

        await IdentityInitializer.InitializeRolesAsync(sp);
        await IdentityInitializer.InitializeDefaultAdminAccountsAsync(sp, configuration);

        await MembershipSeeder.SeedDefaultMemberships(sp);
        await FaqSeeder.SeedDefaultFaq(sp);

        if (env.IsDevelopment())
        {
            await TrainingSessionSeeder.SeedDevSessions(sp);
        }
    }

}
