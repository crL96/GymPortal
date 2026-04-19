namespace Application.Dtos.Faq;

public sealed record FaqItem
(
    int? Id,
    string Title,
    string Content
)
{
    public static FaqItem Create(string title, string content) => new(null, title, content);
    public static FaqItem Create(int id, string title, string content) => new(id, title, content);
}
