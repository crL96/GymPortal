namespace Domain.Common.Exceptions;

public class MissingEmailDomainException() : DomainException("User doesn't have a registered email")
{
}
