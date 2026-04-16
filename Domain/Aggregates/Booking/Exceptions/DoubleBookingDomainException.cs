using Domain.Common.Exceptions;

namespace Domain.Aggregates.Booking.Exceptions;

public class DoubleBookingDomainException() : DomainException("User cannot book the same session twice")
{

}