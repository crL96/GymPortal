using Application.Abstractions.Repositories.Bookings;
using Application.Abstractions.Repositories.Memberships;
using Application.Abstractions.Repositories.TrainingSessions;
using Application.Dtos.Bookings;
using Application.Services;
using Moq;

namespace Tests.Application.Bookings;

public class BookingService_GetUserUpcomingBookings_Tests
{
    [Fact]
    public async Task GetUserUpcomingBookings_Should_Return_List_When_Found()
    {
        var bookingRepo = new Mock<IBookingRepository>();
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();

        bookingRepo.Setup(r => r.GetByUserId(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<BookingDto>
            {
                    BookingDto.Create(
                        Guid.NewGuid(),
                        "user-1",
                        Guid.NewGuid(),
                        "Session",
                        DateTime.Now,
                        DateTime.Now.AddHours(1))
            });

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.GetUserUpcomingBookings("user-1");

        Assert.True(result.Succeeded);
        Assert.NotNull(result.Bookings);
        Assert.Single(result.Bookings);
    }

    [Fact]
    public async Task GetUserUpcomingBookings_Should_Return_Empty_List()
    {
        var bookingRepo = new Mock<IBookingRepository>();
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();

        bookingRepo.Setup(r => r.GetByUserId(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<BookingDto>());

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.GetUserUpcomingBookings("user-1");

        Assert.True(result.Succeeded);
        Assert.Empty(result.Bookings!);
    }

    [Fact]
    public async Task GetUserUpcomingBookings_Should_Return_Failed_When_Invalid_User()
    {
        var bookingRepo = new Mock<IBookingRepository>();
        var sessionRepo = new Mock<ITrainingSessionRepository>();
        var userMembershipRepo = new Mock<IUserMembershipRepository>();

        var service = new BookingService(sessionRepo.Object, userMembershipRepo.Object, bookingRepo.Object);

        var result = await service.GetUserUpcomingBookings("");

        Assert.False(result.Succeeded);
    }
}
