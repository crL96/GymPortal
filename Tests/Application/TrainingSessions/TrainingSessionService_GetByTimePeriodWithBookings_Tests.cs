using Application.Abstractions.Repositories.TrainingSessions;
using Application.Abstractions.Services.Auth;
using Application.Services;
using Application.Dtos.TrainingSessions;
using Moq;

namespace Tests.Application.TrainingSessions;

public class TrainingSessionService_GetByTimePeriodWithBookings_Tests
{
    [Fact]
    public async Task GetByTimePeriod_Should_Return_Sessions_When_Found()
    {
        // Arrange
        var repoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        var sessions = new List<TrainingSessionDto>
        {
            TrainingSessionDto.Create(Guid.NewGuid(), "Test", DateTime.Now, DateTime.Now.AddHours(1), 10, new List<string>())
        };

        repoMock
            .Setup(r => r.GetByTimePeriodWithBookings(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sessions);

        var service = new TrainingSessionService(repoMock.Object, authMock.Object);

        // Act
        var result = await service.GetByTimePeriodWithBookings(DateTime.Now, DateTime.Now.AddDays(1));

        // Assert
        Assert.True(result.Succeeded);
        Assert.Single(result.Sessions!);
    }

    [Fact]
    public async Task GetByTimePeriod_Should_Return_Empty_List_When_No_Data()
    {
        // Arrange
        var repoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        repoMock
            .Setup(r => r.GetByTimePeriodWithBookings(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TrainingSessionDto>());

        var service = new TrainingSessionService(repoMock.Object, authMock.Object);

        // Act
        var result = await service.GetByTimePeriodWithBookings(DateTime.Now, DateTime.Now.AddDays(1));

        // Assert
        Assert.True(result.Succeeded);
        Assert.Empty(result.Sessions!);
    }

    [Fact]
    public async Task GetByTimePeriod_Should_Return_Failed_When_Exception_Is_Thrown()
    {
        // Arrange
        var repoMock = new Mock<ITrainingSessionRepository>();
        var authMock = new Mock<IAuthService>();

        repoMock
            .Setup(r => r.GetByTimePeriodWithBookings(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB crash"));

        var service = new TrainingSessionService(repoMock.Object, authMock.Object);

        // Act
        var result = await service.GetByTimePeriodWithBookings(DateTime.Now, DateTime.Now.AddDays(1));

        // Assert
        Assert.False(result.Succeeded);
        Assert.NotNull(result.ErrorMessage);
    }
}
