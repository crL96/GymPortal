using Domain.Common.Exceptions;

namespace Domain.Aggregates.Membership.Exceptions;

public class InvalidPriceDomainException(string message) : DomainException(message)
{
}
