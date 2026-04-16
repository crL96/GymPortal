namespace Presentation.WebApp.Areas.Account.Models;

public class Membership
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Price { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsActive { get; set; }
}
