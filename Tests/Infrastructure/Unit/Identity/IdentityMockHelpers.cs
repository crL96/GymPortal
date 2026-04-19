using System;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Infrastructure.Unit.Identity;

public class IdentityMockHelpers
{
    internal static UserManager<AppUser> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<AppUser>>();
        return new Mock<UserManager<AppUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!
        ).Object;
    }

    internal static SignInManager<AppUser> CreateSignInManagerMock(UserManager<AppUser> userManager)
    {
        return new Mock<SignInManager<AppUser>>(
            userManager,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null!, null!, null!, null!
        ).Object;
    }

    internal static RoleManager<IdentityRole> CreateRoleManagerMock()
    {
        var store = new Mock<IRoleStore<IdentityRole>>();
        return new Mock<RoleManager<IdentityRole>>(
            store.Object, null!, null!, null!, null!
        ).Object;
    }
}
