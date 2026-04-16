using System;
using Domain.Common.Exceptions;

namespace Domain.Aggregates.Booking.Exceptions;

public class InvalidMembershipDomainException() : DomainException("User must have a valid membership to create booking")
{

}
