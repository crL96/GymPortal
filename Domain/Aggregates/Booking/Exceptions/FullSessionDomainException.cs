using Domain.Common.Exceptions;

namespace Domain.Aggregates.Booking.Exceptions;

public class FullSessionDomainException() : DomainException("Session is full")
{

}