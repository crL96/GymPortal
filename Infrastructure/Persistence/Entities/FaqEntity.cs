namespace Infrastructure.Persistence.Entities;

public class FaqEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}
