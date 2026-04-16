using Domain.Common.Exceptions;

namespace Domain.Aggregates.TrainingSessions.Exceptions;

public class InvalidTimeDomainException(string message) : DomainException(message)
{

}
