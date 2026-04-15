using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class IdentityInitializer
{
    public static async Task InitializeRolesAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new List<IdentityRole>()
        {
            new("Admin"),
            new("Member"),
        };

        foreach (var role in roles)
        {
            if (!string.IsNullOrWhiteSpace(role.Name) && !await roleManager.RoleExistsAsync(role.Name))
                await roleManager.CreateAsync(role);
        }
    }

    public static async Task InitializeDefaultAdminAccountsAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (await userManager.Users.AnyAsync())
            return;

        var admins = new List<string>()
        {
            "admin@corefitness.com",
        };

        var defaultRoleName = "Admin";
        var defaultPassword = configuration.GetValue<string>("DefaultAdminPassword");
        if (string.IsNullOrWhiteSpace(defaultPassword))
            throw new InvalidOperationException("Missing 'DefaultAdminPassword' in secrets or appsettings");

        foreach (var admin in admins)
        {
            var user = AppUser.Create(admin);
            user.EmailConfirmed = true;

            var created = await userManager.CreateAsync(user, defaultPassword);

            if (created.Succeeded && await roleManager.RoleExistsAsync(defaultRoleName))
                await userManager.AddToRoleAsync(user, defaultRoleName);
        }

    }
}
