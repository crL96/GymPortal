using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Infrastructure.Unit.Identity;

public class IdentityAuthService_SignIn_Tests
{
    [Fact]
    public async Task SignIn_Should_Return_InvalidCredentials_When_Input_Invalid()
    {
        var userManager = IdentityMockHelpers.CreateUserManagerMock();
        var signInManager = IdentityMockHelpers.CreateSignInManagerMock(userManager);
        var roleManager = IdentityMockHelpers.CreateRoleManagerMock();

        var service = new IdentityAuthService(signInManager, userManager, roleManager);

        var result = await service.SignInUserAsync("", "");

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task SignIn_Should_Return_LockedOut()
    {
        var userManager = IdentityMockHelpers.CreateUserManagerMock();

        var signInManagerMock = new Mock<SignInManager<AppUser>>(
            userManager,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null!, null!, null!, null!
        );

        signInManagerMock
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.LockedOut);

        var service = new IdentityAuthService(signInManagerMock.Object, userManager, IdentityMockHelpers.CreateRoleManagerMock());

        var result = await service.SignInUserAsync("test@test.com", "password");

        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task SignIn_Should_Return_Ok_When_Success()
    {
        var userManager = IdentityMockHelpers.CreateUserManagerMock();

        var signInManagerMock = new Mock<SignInManager<AppUser>>(
            userManager,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null!, null!, null!, null!
        );

        signInManagerMock
            .Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        var service = new IdentityAuthService(signInManagerMock.Object, userManager, IdentityMockHelpers.CreateRoleManagerMock());

        var result = await service.SignInUserAsync("test@test.com", "password");

        Assert.True(result.Succeeded);
    }
}
