using Application.Dtos.CustomerService;

namespace Application.Abstractions.Services.CustomerService;

public interface IContactService
{
    Task<ContactRequestResult> SaveContactRequest(ContactRequest request, CancellationToken ct = default);
}
