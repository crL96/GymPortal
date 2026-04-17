using Application.Abstractions.Repositories.CustomerService;
using Application.Abstractions.Services.CustomerService;
using Application.Dtos.CustomerService;

namespace Application.Services;

public class ContactService(IContactRequestRepository repo) : IContactService
{
    public async Task<ContactRequestResult> SaveContactRequest(ContactRequest request, CancellationToken ct = default)
    {
        if (!request.AcceptsTerms)
            return ContactRequestResult.Failed("User must accept terms");

        var created = await repo.CreateAsync(request, ct);
        if (created is null)
            return ContactRequestResult.Failed("Could not save to database");

        return ContactRequestResult.Ok();
    }
}
