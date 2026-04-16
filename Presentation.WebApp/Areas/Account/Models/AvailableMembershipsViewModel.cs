namespace Presentation.WebApp.Areas.Account.Models;

public class AvailableMembershipsViewModel
{
    public List<Membership> Memberships { get; set; } = [];
    public string? ErrorMessage { get; set; }
}
