using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data;

internal static class PersistenceInitializer
{
    public static async Task InitializeDatabaseAsync(
        IServiceProvider sp,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(sp);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(env);

        await using var scope = sp.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (env.IsDevelopment())
        {
            await context.Database.EnsureCreatedAsync();
        }
    }

}
