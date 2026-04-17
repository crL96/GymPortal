using Application.Abstractions.Repositories.CustomerService;
using Application.Dtos.CustomerService;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Repositories.CustomerService;

public class ContactRequestRepository(ApplicationDbContext context) :
    RepositoryBase<ContactRequest, int, ContactRequestEntity, ApplicationDbContext>(context),
    IContactRequestRepository
{
    protected override ContactRequestEntity ToEntity(ContactRequest model)
    {
        return new ContactRequestEntity()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Message = model.Message,
            CreatedAt = DateTime.UtcNow
        };
    }

    protected override ContactRequest ToModel(ContactRequestEntity entity)
    {
        return new ContactRequest(
            entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.Email,
            entity.PhoneNumber,
            entity.Message,
            true,
            entity.CreatedAt
        );
    }
}
