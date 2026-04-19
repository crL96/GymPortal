using Application.Abstractions.Repositories.Bookings;
using Application.Abstractions.Repositories.Memberships;
using Application.Abstractions.Repositories.TrainingSessions;
using Application.Services;
using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.Exceptions;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.Membership.ValueObjects;
using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Aggregates.UserMemberships;
using Moq;

namespace Tests.Application.Bookings;

public class BookingService_CreateBooking_Tests
{
    [Fact]
    public async Task CreateBooking_Should_Return_Ok_When_Success()
    {
        // Arrange
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        var session = TrainingSession.Create("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10);

        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        userMembershipRepo.Setup(r => r.GetByIdAsync("user-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserMembership.Create("user-1", MembershipId.Create()));

        bookingRepo.Setup(r => r.GetBySessionId(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Booking>());

        bookingRepo.Setup(r => r.CreateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Booking.Recreate(BookingId.Create(), "user-1", session.Id));

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        // Act
        var result = await service.CreateBooking(Guid.NewGuid(), "user-1");

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task CreateBooking_Should_Return_InvalidUserId_When_Empty()
    {
        // Arrange
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        // Act
        var result = await service.CreateBooking(Guid.NewGuid(), "");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Invalid UserId", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_Should_Return_NotFound_When_Session_Missing()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TrainingSession?)null);

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.CreateBooking(Guid.NewGuid(), "user-1");

        Assert.False(result.Succeeded);
        Assert.Equal("Session not found", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_Should_Return_Failed_When_Repo_Fails()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        var session = TrainingSession.Create("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10);

        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        userMembershipRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserMembership.Create("user-1", MembershipId.Create()));

        bookingRepo.Setup(r => r.GetBySessionId(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Booking>());

        bookingRepo.Setup(r => r.CreateAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking?)null);

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.CreateBooking(Guid.NewGuid(), "user-1");

        Assert.False(result.Succeeded);
        Assert.Equal("Failed to save booking to database", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_Should_Return_UserAlreadyBooked()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        var session = TrainingSession.Create("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10);

        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        userMembershipRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserMembership.Create("user-1", MembershipId.Create()));

        bookingRepo.Setup(r => r.GetBySessionId(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Booking>
            {
                Booking.Recreate(BookingId.Create(), "user-1", session.Id)
            });

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.CreateBooking(Guid.NewGuid(), "user-1");

        Assert.False(result.Succeeded);
        Assert.Equal("User is already booked to session", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_Should_Return_SessionFull()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        var session = TrainingSession.Create("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 1);

        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        userMembershipRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(UserMembership.Create("user-1", MembershipId.Create()));

        bookingRepo.Setup(r => r.GetBySessionId(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Booking>
            {
                Booking.Recreate(BookingId.Create(), "user-2", session.Id)
            });

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.CreateBooking(Guid.NewGuid(), "user-1");

        Assert.False(result.Succeeded);
        Assert.Equal("Session is full", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_Should_Return_InvalidMembership()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        var session = TrainingSession.Create("Test", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(1), 10);

        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        userMembershipRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserMembership?)null);

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.CreateBooking(Guid.NewGuid(), "user-1");

        Assert.False(result.Succeeded);
        Assert.Equal("User does not have a valid membership", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateBooking_Should_Return_Exception()
    {
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();
        var bookingRepo = new Mock<IBookingRepository>();

        sessionRepo.Setup(r => r.GetByIdAsync(It.IsAny<TrainingSessionId>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB crash"));

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.CreateBooking(Guid.NewGuid(), "user-1");

        Assert.False(result.Succeeded);
    }
}
