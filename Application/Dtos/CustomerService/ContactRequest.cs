namespace Application.Dtos.CustomerService;

public sealed record ContactRequest
(
    int? Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string Message,
    bool AcceptsTerms,
    DateTime CreatedAt
);
