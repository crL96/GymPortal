using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Common.Exceptions;

namespace Tests.Domain.Bookings;

public class BookingTests
{
    [Fact]
    public void Recreate_Should_Create_Booking_With_Provided_Values()
    {
        // Arrange
        var id = BookingId.Create();
        var userId = "user-1";
        var trainingSessionId = TrainingSessionId.Create();

        // Act
        var booking = Booking.Recreate(id, userId, trainingSessionId);

        // Assert
        Assert.Equal(id, booking.Id);
        Assert.Equal(userId, booking.UserId);
        Assert.Equal(trainingSessionId, booking.TrainingSessionId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Recreate_Should_Throw_When_UserId_Is_Invalid(string userId)
    {
        // Arrange
        var id = BookingId.Create();
        var trainingSessionId = TrainingSessionId.Create();

        // Act
        Action act = () => Booking.Recreate(id, userId, trainingSessionId);

        // Assert
        Assert.Throws<InvalidIdDomainException>(act);
    }
}
