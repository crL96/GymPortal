using Application.Dtos.Faq;

namespace Application.Abstractions.Services.Faq;

public interface IFaqService
{
    Task<FaqResult> GetFaqsAsync();
}
