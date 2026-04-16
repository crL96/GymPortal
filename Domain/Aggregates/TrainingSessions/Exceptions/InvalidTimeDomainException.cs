using Domain.Common.Exceptions;

namespace Domain.Aggregates.TrainingSessions.Exceptions;

public class InvalidTimeDomainException(string message) : DomainException(message)
{

}

public class NoAvailableSpotsDomainException() : DomainException("Session must have atleast 1 available spot")
{

}