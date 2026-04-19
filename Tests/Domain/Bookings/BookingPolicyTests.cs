using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.Exceptions;
using Domain.Aggregates.Membership;
using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.UserMemberships;

namespace Tests.Domain.Bookings;

public class BookingPolicyTests
{
    [Fact]
    public void CreateBooking_Should_Create_Booking_When_All_Conditions_Are_Met()
    {
        // Arrange
        var session = TrainingSession.Create(
            "Test",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1),
            10);

        var membership = Membership.Create("Premium", 100);
        var userMembership = UserMembership.Create("user-1", membership.Id);

        var bookedUserIds = new List<string>();

        // Act
        var booking = BookingPolicy.CreateBooking(session, userMembership, bookedUserIds);

        // Assert
        Assert.Equal("user-1", booking.UserId);
        Assert.Equal(session.Id, booking.TrainingSessionId);
    }

    [Fact]
    public void CreateBooking_Should_Throw_When_UserMembership_Is_Null()
    {
        // Arrange
        var session = TrainingSession.Create(
            "Test",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1),
            10);

        List<string> bookedUserIds = new();

        // Act
        Action act = () => BookingPolicy.CreateBooking(session, null, bookedUserIds);

        // Assert
        Assert.Throws<InvalidMembershipDomainException>(act);
    }

    [Fact]
    public void CreateBooking_Should_Throw_When_UserMembership_Is_Inactive()
    {
        // Arrange
        var session = TrainingSession.Create(
            "Test",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1),
            10);

        var membership = Membership.Create("Premium", 100);
        var userMembership = UserMembership.Create("user-1", membership.Id);
        userMembership.CancelPlan();

        List<string> bookedUserIds = new();

        // Act
        Action act = () => BookingPolicy.CreateBooking(session, userMembership, bookedUserIds);

        // Assert
        Assert.Throws<InvalidMembershipDomainException>(act);
    }

    [Fact]
    public void CreateBooking_Should_Throw_When_Session_Is_Full()
    {
        // Arrange
        var session = TrainingSession.Create(
            "Test",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1),
            1);

        var membership = Membership.Create("Premium", 100);
        var userMembership = UserMembership.Create("user-1", membership.Id);

        var bookedUserIds = new List<string> { "user-x" };

        // Act
        Action act = () => BookingPolicy.CreateBooking(session, userMembership, bookedUserIds);

        // Assert
        Assert.Throws<FullSessionDomainException>(act);
    }

    [Fact]
    public void CreateBooking_Should_Throw_When_User_Already_Booked()
    {
        // Arrange
        var session = TrainingSession.Create(
            "Test",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1),
            10);

        var membership = Membership.Create("Premium", 100);
        var userMembership = UserMembership.Create("user-1", membership.Id);

        var bookedUserIds = new List<string> { "user-1" };

        // Act
        Action act = () => BookingPolicy.CreateBooking(session, userMembership, bookedUserIds);

        // Assert
        Assert.Throws<DoubleBookingDomainException>(act);
    }

}
