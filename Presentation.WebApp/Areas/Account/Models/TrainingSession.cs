namespace Presentation.WebApp.Areas.Account.Models;

public class TrainingSession
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int AvailableSpots { get; set; }
    public List<string> BookedIds { get; set; } = [];
}
