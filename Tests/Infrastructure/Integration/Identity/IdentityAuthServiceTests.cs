using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration.Identity;

public class IdentityAuthServiceTests
{
    private IdentityAuthService CreateService(out UserManager<AppUser> userManager)
    {
        var provider = TestDbFactory.CreateProvider();

        userManager = provider.GetRequiredService<UserManager<AppUser>>();
        var signInManager = provider.GetRequiredService<SignInManager<AppUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        roleManager.CreateAsync(new("Admin"));
        roleManager.CreateAsync(new("Member"));

        return new IdentityAuthService(signInManager, userManager, roleManager);
    }

    [Fact]
    public async Task SignUpUserAsync_Should_Return_Success()
    {
        var service = CreateService(out var userManager);

        var result = await service.SignUpUserAsync("test@test.com", "Password123!");

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task SignUpUserAsync_Should_Return_InvalidCredentials()
    {
        var service = CreateService(out var userManager);

        var result = await service.SignUpUserAsync("", "");

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task SignUpUserAsync_Should_Return_UserAlreadyExists()
    {
        var service = CreateService(out var userManager);

        await service.SignUpUserAsync("test@test.com", "Password123!");

        var result = await service.SignUpUserAsync("test@test.com", "Password123!");

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task SignInUserAsync_Should_Return_InvalidCredentials()
    {
        var service = CreateService(out var userManager);

        var result = await service.SignInUserAsync("test@test.com", "Password123!");

        Assert.False(result.Succeeded);
    }

    [Theory]
    [InlineData("", "Password123!")]
    [InlineData("test@test.com", "")]
    [InlineData("", "")]
    public async Task SignInUserAsync_Should_Return_InvalidCredentials_For_EmptyInput(string email, string password)
    {
        var service = CreateService(out var userManager);

        var result = await service.SignInUserAsync(email, password);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task UserExists_Should_Return_True_When_User_Exists()
    {
        var service = CreateService(out var userManager);

        await service.SignUpUserAsync("test@test.com", "Password123!");

        var result = await service.UserExists("test@test.com");

        Assert.True(result);
    }

    [Fact]
    public async Task UserExists_Should_Return_False_When_User_Does_Not_Exist()
    {
        var service = CreateService(out var userManager);

        var result = await service.UserExists("missing@test.com");

        Assert.False(result);
    }

    [Fact]
    public async Task UserExists_Should_Throw_On_Invalid_Input()
    {
        var service = CreateService(out var userManager);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            service.UserExists(""));
    }

    [Fact]
    public async Task IsUserAdmin_Should_Return_True_When_User_Is_Admin()
    {
        var service = CreateService(out var userManager);
        var provider = TestDbFactory.CreateProvider();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        await roleManager.CreateAsync(new IdentityRole("Admin"));

        var userResult = await service.SignUpUserAsync("admin@test.com", "Password123!");

        var user = await userManager.FindByEmailAsync("admin@test.com");
        await userManager.AddToRoleAsync(user!, "Admin");

        var result = await service.IsUserAdmin(user!.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task IsUserAdmin_Should_Return_False_When_User_Not_Admin()
    {
        var service = CreateService(out var userManager);

        await service.SignUpUserAsync("user@test.com", "Password123!");

        var user = await userManager.FindByEmailAsync("user@test.com");

        var result = await service.IsUserAdmin(user!.Id);

        Assert.False(result);
    }

    [Fact]
    public async Task IsUserAdmin_Should_Return_False_When_User_Not_Found()
    {
        var service = CreateService(out var userManager);

        var result = await service.IsUserAdmin(Guid.NewGuid().ToString());

        Assert.False(result);
    }
}
