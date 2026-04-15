namespace Domain.Common.Exceptions;

public class NullOrWhitespaceDomainException(string message) : DomainException(message)
{
}