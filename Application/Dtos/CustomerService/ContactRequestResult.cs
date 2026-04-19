namespace Application.Dtos.CustomerService;

public sealed record ContactRequestResult(bool Succeeded, string? Message = null)
{
    public static ContactRequestResult Ok() => new(true);
    public static ContactRequestResult Failed(string message) => new(false, message);
}
