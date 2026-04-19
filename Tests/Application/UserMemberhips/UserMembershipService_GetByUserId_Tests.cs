using Application.Abstractions.Repositories.Memberships;
using Application.Services;
using Domain.Aggregates.Membership;
using Domain.Aggregates.UserMemberships;
using Moq;

namespace Tests.Application.UserMemberhips;

public class UserMembershipService_GetByUserId_Tests
{
    [Fact]
    public async Task GetByUserId_Should_Return_UserMembership_When_Found()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        var membership = Membership.Create("Premium", 100);
        var userMembership = UserMembership.Create(userId, membership.Id);

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userMembership);

        membershipRepoMock
            .Setup(r => r.GetByIdAsync(userMembership.MembershipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(membership);

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.GetByUserIdAsync(userId);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.UserMembership);
        Assert.Equal(userId, result.UserMembership!.UserId);
    }

    [Fact]
    public async Task GetByUserId_Should_Return_NotFound_When_UserMembership_Is_Null()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserMembership?)null);

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.GetByUserIdAsync(userId);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Membership not found", result.ErrorMessage);
    }

    [Fact]
    public async Task GetByUserId_Should_Return_NotFound_When_MembershipDetails_Is_Null()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        var membership = Membership.Create("Premium", 100);
        var userMembership = UserMembership.Create(userId, membership.Id);

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userMembership);

        membershipRepoMock
            .Setup(r => r.GetByIdAsync(userMembership.MembershipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Membership?)null);

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.GetByUserIdAsync(userId);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Membership not found", result.ErrorMessage);
    }

    [Fact]
    public async Task GetByUserId_Should_Return_Failed_When_Exception_Is_Thrown()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB crash"));

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.GetByUserIdAsync(userId);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("DB crash", result.ErrorMessage);
    }
}