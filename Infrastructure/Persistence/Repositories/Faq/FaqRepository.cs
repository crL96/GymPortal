using Application.Abstractions.Repositories.Faq;
using Application.Dtos.Faq;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Repositories.Faq;

public class FaqRepository(ApplicationDbContext context) : RepositoryBase<FaqItem, int, FaqEntity, ApplicationDbContext>(context), IFaqRepository
{
    protected override FaqEntity ToEntity(FaqItem model)
    {
        return new FaqEntity()
        {
            Title = model.Title,
            Content = model.Content
        };
    }

    protected override FaqItem ToModel(FaqEntity entity)
    {
        return FaqItem.Create(entity.Id, entity.Title, entity.Content);
    }
}
