using Application.Abstractions.Repositories.Memberships;
using Application.Services;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Aggregates.UserMemberships;
using Moq;

namespace Tests.Application.UserMemberhips;

public class UserMembershipService_CancelMembership_Tests
{
    [Fact]
    public async Task CancelMembership_Should_Return_Ok_On_Success()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        var userMembership = UserMembership.Create(userId, MembershipId.Create());

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userMembership);

        repoMock
            .Setup(r => r.UpdateAsync(userId, userMembership, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.CancelMembership(userId);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task CancelMembership_Should_Return_NotFound_When_UserMembership_Does_Not_Exist()
    {
        // Arrange
        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserMembership?)null);

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.CancelMembership("user-1");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Membership not found", result.ErrorMessage);
    }

    [Fact]
    public async Task CancelMembership_Should_Return_Failed_When_Update_Fails()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        var userMembership = UserMembership.Create(userId, MembershipId.Create());

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userMembership);

        repoMock
            .Setup(r => r.UpdateAsync(userId, userMembership, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.CancelMembership(userId);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Could not save to database", result.ErrorMessage);
    }

    [Fact]
    public async Task CancelMembership_Should_Return_Failed_When_Exception_Is_Thrown()
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
        var result = await service.CancelMembership(userId);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("DB crash", result.ErrorMessage);
    }
}
