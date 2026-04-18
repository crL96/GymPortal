using Presentation.WebApp.Models.Common;

namespace Presentation.WebApp.Areas.Admin.Models;

public class SessionPageViewModel
{
    public List<TrainingSession> Sessions { get; set; } = [];
    public CreateSessionForm CreateSessionForm { get; set; } = new();
}
