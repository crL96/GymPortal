using System;
using Domain.Common.Exceptions;

namespace Domain.Aggregates.Booking.ValueObjects;

public class BookingId
{
    public Guid Value { get; private set; }

    public static BookingId Create()
    {
        return new()
        {
            Value = Guid.NewGuid()
        };
    }

    public static BookingId Recreate(Guid id)
    {
        if (id == Guid.Empty)
            throw new InvalidIdDomainException("Id cannot be an empty GUID");

        return new()
        {
            Value = id
        };
    }
}
