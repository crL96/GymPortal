using Application.Abstractions.Repositories.TrainingSessions;
using Application.Abstractions.Services.Auth;
using Application.Dtos.TrainingSessions;
using Application.Services;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Common.Exceptions;
using Moq;

namespace Tests.Application.TrainingSessions;

public class TrainingSessionService_DeleteSession_Tests
{
    [Fact]
    public async Task DeleteSession_Should_Return_Ok_When_Success()
    {
        // Arrange
        var repoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock.Setup(a => a.IsUserAdmin(It.IsAny<string>())).ReturnsAsync(true);

        repoMock
            .Setup(r => r.DeleteAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = new TrainingSessionService(repoMock.Object, authMock.Object);

        // Act
        var result = await service.DeleteSessionAsync(Guid.NewGuid(), "user-1");

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteSession_Should_Return_Unauthorized_When_Not_Admin()
    {
        // Arrange
        var repoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock.Setup(a => a.IsUserAdmin(It.IsAny<string>())).ReturnsAsync(false);

        var service = new TrainingSessionService(repoMock.Object, authMock.Object);

        // Act
        var result = await service.DeleteSessionAsync(Guid.NewGuid(), "user-1");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(DeleteSessionErrorType.Unauthorized, result.ErrorType);
    }

    [Fact]
    public async Task DeleteSession_Should_Return_Failed_When_Repository_Fails()
    {
        // Arrange
        var repoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock.Setup(a => a.IsUserAdmin(It.IsAny<string>())).ReturnsAsync(true);

        repoMock
            .Setup(r => r.DeleteAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new TrainingSessionService(repoMock.Object, authMock.Object);

        // Act
        var result = await service.DeleteSessionAsync(Guid.NewGuid(), "user-1");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Failed to delete session", result.ErrorMessage);
    }

    [Fact]
    public async Task DeleteSession_Should_Return_InvalidId_When_Exception_Is_Thrown()
    {
        // Arrange
        var repoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock.Setup(a => a.IsUserAdmin(It.IsAny<string>())).ReturnsAsync(true);

        repoMock
            .Setup(r => r.DeleteAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidIdDomainException("message"));

        var service = new TrainingSessionService(repoMock.Object, authMock.Object);

        // Act
        var result = await service.DeleteSessionAsync(Guid.NewGuid(), "user-1");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(DeleteSessionErrorType.InvalidId, result.ErrorType);
    }
}
