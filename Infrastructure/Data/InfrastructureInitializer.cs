using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public static class InfrastructureInitializer
{
    public static async Task InitializeAsync(IServiceProvider sp, IConfiguration configuration, IWebHostEnvironment env)
    {
        await PersistenceInitializer.InitializeDatabaseAsync(sp, configuration, env);

        await IdentityInitializer.InitializeRolesAsync(sp);
        await IdentityInitializer.InitializeDefaultAdminAccountsAsync(sp, configuration);
    }

}
