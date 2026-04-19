using Application.Abstractions.Repositories.TrainingSessions;
using Application.Abstractions.Services.Auth;
using Application.Services;
using Application.Dtos.TrainingSessions;
using Domain.Aggregates.TrainingSessions;
using Moq;

namespace Tests.Application.TrainingSessions;

public class TrainingSessionService_CreateSession_Tests
{
    [Fact]
    public async Task CreateSession_Should_Return_Ok_When_Success()
    {
        // Arrange
        var sessionRepoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock
            .Setup(a => a.IsUserAdmin(It.IsAny<string>()))
            .ReturnsAsync(true);

        sessionRepoMock
            .Setup(r => r.CreateAsync(It.IsAny<TrainingSession>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TrainingSession.Create("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10));

        var service = new TrainingSessionService(sessionRepoMock.Object, authMock.Object);

        var dto = new CreateSessionDto("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10);

        // Act
        var result = await service.CreateSessionAsync(dto, "user-1");

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task CreateSession_Should_Fail_When_User_Is_Not_Admin()
    {
        // Arrange
        var sessionRepoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock
            .Setup(a => a.IsUserAdmin(It.IsAny<string>()))
            .ReturnsAsync(false);

        var service = new TrainingSessionService(sessionRepoMock.Object, authMock.Object);

        var dto = new CreateSessionDto("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10);

        // Act
        var result = await service.CreateSessionAsync(dto, "user-1");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("User is not an admin", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateSession_Should_Fail_When_Repository_Returns_Null()
    {
        // Arrange
        var sessionRepoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock.Setup(a => a.IsUserAdmin(It.IsAny<string>())).ReturnsAsync(true);

        sessionRepoMock
            .Setup(r => r.CreateAsync(It.IsAny<TrainingSession>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrainingSession?)null);

        var service = new TrainingSessionService(sessionRepoMock.Object, authMock.Object);

        var dto = new CreateSessionDto("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10);

        // Act
        var result = await service.CreateSessionAsync(dto, "user-1");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Failed to save session to database.", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateSession_Should_Fail_When_Domain_Exception_Is_Thrown()
    {
        // Arrange
        var sessionRepoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        authMock.Setup(a => a.IsUserAdmin(It.IsAny<string>())).ReturnsAsync(true);

        var service = new TrainingSessionService(sessionRepoMock.Object, authMock.Object);

        // Invalid start time -> domain exception
        var dto = new CreateSessionDto("Test", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), 10);

        // Act
        var result = await service.CreateSessionAsync(dto, "user-1");

        // Assert
        Assert.False(result.Succeeded);
        Assert.NotNull(result.ErrorMessage);
    }
}
