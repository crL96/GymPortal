using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, IdentityRole>(x =>
        {
            x.SignIn.RequireConfirmedAccount = false;
            x.User.RequireUniqueEmail = true;
            x.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(x =>
        {
            x.LoginPath = "/sign-in";
            x.LogoutPath = "/";
            x.AccessDeniedPath = "/denied";

            x.Cookie.IsEssential = true;
            x.Cookie.Name = "gymportal_auth";
            x.ExpireTimeSpan = TimeSpan.FromDays(30);
            x.SlidingExpiration = true;
        });

        return services;
    }
}
