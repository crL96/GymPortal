namespace Application.Dtos.Faq;

public sealed record FaqResult
(
    bool Succeeded,
    IReadOnlyList<FaqItem>? Faqs = null,
    string? ErrorMessage = null
)
{
    public static FaqResult Ok(IReadOnlyList<FaqItem> faqs) => new(true, faqs);
    public static FaqResult Failed(string errorMessage) => new(false, null, errorMessage);
}
