using System;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Common.Exceptions;

namespace Tests.Domain.Bookings;

public class BookingIdTests
{
    [Fact]
    public void Create_Should_Return_Valid_BookingId()
    {
        // Act
        var id = BookingId.Create();

        // Assert
        Assert.IsType<Guid>(id.Value);
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void Create_Should_Return_Unique_Ids()
    {
        // Act
        var id1 = BookingId.Create();
        var id2 = BookingId.Create();

        // Assert
        Assert.NotEqual(id1.Value, id2.Value);
    }

    [Fact]
    public void Recreate_Should_Return_Id_When_Guid_Is_Valid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var id = BookingId.Recreate(guid);

        // Assert
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void Recreate_Should_Throw_When_Guid_Is_Empty()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act & Assert
        Assert.Throws<InvalidIdDomainException>(() =>
            BookingId.Recreate(guid));
    }

}
