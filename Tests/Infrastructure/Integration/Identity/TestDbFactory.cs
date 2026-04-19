using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration.Identity;

public static class TestDbFactory
{
    public static ServiceProvider CreateProvider()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=:memory:;"));

        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddLogging();

        var provider = services.BuildServiceProvider();

        var context = provider.GetRequiredService<ApplicationDbContext>();
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return provider;
    }
}
