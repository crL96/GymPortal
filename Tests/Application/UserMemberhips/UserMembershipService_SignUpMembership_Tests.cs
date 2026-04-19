using Application.Abstractions.Repositories.Memberships;
using Application.Services;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Aggregates.UserMemberships;
using Moq;

namespace Tests.Application.UserMemberhips;

public class UserMembershipService_SignUpMembership_Tests
{

    [Fact]
    public async Task SignUpMembership_Should_Return_AlreadyExists_When_User_Has_Membership()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserMembership.Create(userId, MembershipId.Create()));

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.SignUpMembership(userId, Guid.NewGuid());

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("User already has a membership", result.ErrorMessage);
    }

    [Fact]
    public async Task SignUpMembership_Should_Return_Ok_When_Created_Successfully()
    {
        // Arrange
        var userId = "user-1";
        var membershipId = MembershipId.Create().Value;

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserMembership?)null);

        repoMock
            .Setup(r => r.CreateAsync(It.IsAny<UserMembership>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserMembership.Recreate(userId, MembershipId.Create(), true));

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.SignUpMembership(userId, membershipId);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task SignUpMembership_Should_Return_Failed_When_Repo_Return_Null()
    {
        // Arrange
        var userId = "user-1";
        var membershipId = MembershipId.Create().Value;

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserMembership?)null);

        repoMock
            .Setup(r => r.CreateAsync(It.IsAny<UserMembership>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserMembership?)null);

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.SignUpMembership(userId, membershipId);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Could not save to database", result.ErrorMessage);
    }

    [Fact]
    public async Task SignUpMembership_Should_Return_InvalidId_When_Domain_Exception_Is_Thrown()
    {
        // Arrange
        var userId = "user-1";

        var repoMock = new Mock<IUserMembershipRepository>();
        var membershipRepoMock = new Mock<IMembershipRepository>();

        repoMock
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserMembership?)null);

        var invalidGuid = Guid.Empty;

        var service = new UserMembershipService(repoMock.Object, membershipRepoMock.Object);

        // Act
        var result = await service.SignUpMembership(userId, invalidGuid);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Id cannot be an empty GUID", result.ErrorMessage);
    }
}
