using Application.Abstractions.Repositories.Bookings;
using Application.Abstractions.Repositories.Memberships;
using Application.Abstractions.Repositories.TrainingSessions;
using Application.Services;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Moq;

namespace Tests.Application.Bookings;

public class BookingService_RemoveBooking_Tests
{
    [Fact]
    public async Task RemoveBookingByUserAndSession_Should_Return_Ok_When_Success()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        bookingRepo.Setup(r => r.DeleteByUserAndSessionIdAsync(It.IsAny<string>(), It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.RemoveBookingByUserAndSession(Guid.NewGuid(), "user-1");

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RemoveBookingByUserAndSession_Should_Return_Failed_When_Fails()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        bookingRepo.Setup(r => r.DeleteByUserAndSessionIdAsync(It.IsAny<string>(), It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.RemoveBookingByUserAndSession(Guid.NewGuid(), "user-1");

        Assert.False(result.Succeeded);
        Assert.Equal("Failed to remove booking", result.ErrorMessage);
    }

    [Fact]
    public async Task RemoveBooking_Should_Return_Ok_When_Success()
    {
        // Arrange
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        bookingRepo
            .Setup(r => r.DeleteAsync(It.IsAny<BookingId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        // Act
        var result = await service.RemoveBooking(Guid.NewGuid());

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RemoveBooking_Should_Return_Failed_When_Delete_Fails()
    {
        // Arrange
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        bookingRepo
            .Setup(r => r.DeleteAsync(It.IsAny<BookingId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        // Act
        var result = await service.RemoveBooking(Guid.NewGuid());

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Failed to remove booking", result.ErrorMessage);
    }
}
