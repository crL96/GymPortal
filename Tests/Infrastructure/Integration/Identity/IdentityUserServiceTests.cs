using System;
using Application.Dtos.User;
using Domain.Common.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration.Identity;

public class IdentityUserServiceTests
{
    [Fact]
    public async Task GetUserDetailsAsync_Should_Return_Success()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var user = AppUser.Create("test@test.com");
        user.FirstName = "Carl";
        user.LastName = "Carlsson";

        await userManager.CreateAsync(user);

        var service = new IdentityUserService(userManager);

        var result = await service.GetUserDetailsAsync(user.Id);

        Assert.True(result.Succeeded);
        Assert.NotNull(result.UserDetails);
    }

    [Fact]
    public async Task GetUserDetailsAsync_Should_Throw_On_Invalid_UserId()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var service = new IdentityUserService(userManager);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            service.GetUserDetailsAsync(""));
    }

    [Fact]
    public async Task GetUserDetailsAsync_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var service = new IdentityUserService(userManager);

        var result = await service.GetUserDetailsAsync(Guid.NewGuid().ToString());

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task GetUserDetailsAsync_Should_Throw_When_Email_Is_Missing()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var user = AppUser.Create("test@test.com");
        user.Email = null; // simulate missing email
        await userManager.CreateAsync(user);

        var service = new IdentityUserService(userManager);

        await Assert.ThrowsAsync<MissingEmailDomainException>(() =>
            service.GetUserDetailsAsync(user.Id));
    }

    [Fact]
    public async Task UpdateUserDetailsAsync_Should_Throw_On_Null_Input()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var service = new IdentityUserService(userManager);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            service.UpdateUserDetailsAsync(null!));
    }

    [Fact]
    public async Task UpdateUserDetailsAsync_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var service = new IdentityUserService(userManager);

        var dto = new UserDetails(
            Guid.NewGuid().ToString(),
            "test@test.com",
            "Carl",
            "Carlsson",
            null,
            null
        );

        var result = await service.UpdateUserDetailsAsync(dto);

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task UpdateUserDetailsAsync_Should_Return_Success()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var user = AppUser.Create("test@test.com");
        await userManager.CreateAsync(user);

        var service = new IdentityUserService(userManager);

        var dto = new UserDetails(
            user.Id,
            "test@test.com",
            "Carl",
            "Carlsson",
            "123",
            "img.png"
        );

        var result = await service.UpdateUserDetailsAsync(dto);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Throw_On_Invalid_UserId()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var service = new IdentityUserService(userManager);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            service.DeleteUserAsync(""));
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var service = new IdentityUserService(userManager);

        var result = await service.DeleteUserAsync(Guid.NewGuid().ToString());

        Assert.False(result.Succeeded);
    }


    [Fact]
    public async Task DeleteUserAsync_Should_Return_Success_When_User_Is_Deleted()
    {
        var provider = TestDbFactory.CreateProvider();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var user = AppUser.Create("test@test.com");
        await userManager.CreateAsync(user);

        var service = new IdentityUserService(userManager);

        var result = await service.DeleteUserAsync(user.Id);

        Assert.True(result.Succeeded);
    }
}
