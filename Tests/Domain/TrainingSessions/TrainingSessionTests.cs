using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.TrainingSessions.Exceptions;
using Domain.Common.Exceptions;

namespace Tests.Domain.TrainingSessions;

public class TrainingSessionTests
{
    [Fact]
    public void Create_Should_Create_TrainingSession_When_Input_Is_Valid()
    {
        // Arrange
        var name = "Strength Training";
        var startTime = DateTime.Now.AddDays(1);
        var endTime = startTime.AddHours(1);
        var availableSpots = 10;

        // Act
        var session = TrainingSession.Create(name, startTime, endTime, availableSpots);

        // Assert
        Assert.Equal("Strength Training", session.Name);
        Assert.Equal(10, session.AvailableSpots);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Should_Throw_When_Name_Is_Invalid(string name)
    {
        // Arrange
        var startTime = DateTime.Now.AddDays(1);
        var endTime = startTime.AddHours(1);
        var availableSpots = 10;

        // Act & Assert
        Assert.Throws<NullOrWhitespaceDomainException>(() =>
            TrainingSession.Create(name, startTime, endTime, availableSpots));
    }

    [Fact]
    public void Create_Should_Throw_When_StartTime_Is_In_Past()
    {
        // Arrange
        var name = "Test";
        var startTime = DateTime.Now.AddDays(-1);
        var endTime = DateTime.Now.AddDays(1);
        var availableSpots = 10;

        // Act & Assert
        Assert.Throws<InvalidTimeDomainException>(() =>
            TrainingSession.Create(name, startTime, endTime, availableSpots));
    }

    [Fact]
    public void Create_Should_Throw_When_EndTime_Is_Too_Short()
    {
        // Arrange
        var name = "Test";
        var startTime = DateTime.Now.AddDays(1);
        var endTime = startTime.AddMinutes(10);
        var availableSpots = 10;

        // Act & Assert
        Assert.Throws<InvalidTimeDomainException>(() =>
            TrainingSession.Create(name, startTime, endTime, availableSpots));
    }

    [Fact]
    public void Create_Should_Throw_When_AvailableSpots_Is_Less_Than_One()
    {
        // Arrange
        var name = "Test";
        var startTime = DateTime.Now.AddDays(1);
        var endTime = startTime.AddHours(1);
        var availableSpots = 0;

        // Act & Assert
        Assert.Throws<NoAvailableSpotsDomainException>(() =>
            TrainingSession.Create(name, startTime, endTime, availableSpots));
    }

    [Fact]
    public void IsFull_Should_Return_True_When_NumberOfBookings_Equals_Spots()
    {
        // Arrange
        var session = TrainingSession.Create(
            "Test",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1),
            10);

        var numberOfBookings = 10;

        // Act
        var result = session.IsFull(numberOfBookings);

        // Assert
        Assert.True(result);
    }
}
