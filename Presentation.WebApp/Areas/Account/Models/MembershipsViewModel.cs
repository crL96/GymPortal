namespace Presentation.WebApp.Areas.Account.Models;

public class MembershipsViewModel
{
    public AvailableMembershipsViewModel AvailableMembershipsViewModel { get; set; } = null!;
    public Membership? CurrentMembership { get; set; }
}
