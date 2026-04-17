using Application.Abstractions.Repositories.Faq;
using Application.Abstractions.Services.Faq;
using Application.Dtos.Faq;

namespace Application.Services;

public class FaqService(IFaqRepository repo) : IFaqService
{
    public async Task<FaqResult> GetFaqsAsync()
    {
        try
        {
            var faqItems = await repo.GetAllAsync();
            return FaqResult.Ok(faqItems);
        }
        catch
        {
            return FaqResult.Failed("Could not fetch Faqs from database");
        }
    }
}
