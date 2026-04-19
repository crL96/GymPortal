using Application.Abstractions.Repositories.Memberships;
using Application.Services;
using Domain.Aggregates.Membership;
using Domain.Aggregates.Membership.ValueObjects;
using Moq;

namespace Tests.Application.Memberships
{
    public class MembershipServiceTests
    {
        [Fact]
        public async Task GetAllAsync_Should_Return_Memberships_When_Repository_Succeeds()
        {
            // Arrange
            var repoMock = new Mock<IMembershipRepository>();

            var memberships = new List<Membership>
        {
            Membership.Create("Basic", 50),
            Membership.Create("Premium", 100)
        };

            repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(memberships);

            var service = new MembershipService(repoMock.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Memberships);
            Assert.Equal(2, result.Memberships!.Count);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Failed_When_Repository_Throws()
        {
            // Arrange
            var repoMock = new Mock<IMembershipRepository>();

            repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            var service = new MembershipService(repoMock.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Memberships);
            Assert.NotNull(result.ErrorMessage);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Membership_When_Found()
        {
            // Arrange
            var repoMock = new Mock<IMembershipRepository>();

            var membership = Membership.Create("Premium", 100);

            repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<MembershipId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(membership);

            var service = new MembershipService(repoMock.Object);

            // Act
            var result = await service.GetByIdAsync(membership.Id.Value);

            // Assert
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Membership);
            Assert.Equal(membership.Id.Value, result.Membership!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_NotFound_When_Null()
        {
            // Arrange
            var repoMock = new Mock<IMembershipRepository>();

            repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<MembershipId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Membership?)null);

            var service = new MembershipService(repoMock.Object);

            // Act
            var result = await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Membership);
            Assert.Equal("Membership not found", result.ErrorMessage);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Failed_When_Exception_Thrown()
        {
            // Arrange
            var repoMock = new Mock<IMembershipRepository>();

            repoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<MembershipId>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB crash"));

            var service = new MembershipService(repoMock.Object);

            // Act
            var result = await service.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.False(result.Succeeded);
            Assert.Null(result.Membership);
            Assert.NotNull(result.ErrorMessage);
        }
    }
}