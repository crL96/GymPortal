namespace Domain.Common.Exceptions;

public class InvalidIdDomainException(string message) : DomainException(message)
{
}
