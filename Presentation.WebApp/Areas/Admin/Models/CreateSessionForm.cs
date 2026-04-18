using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Admin.Models;

public class CreateSessionForm
{
    [Required(ErrorMessage = "Session name is required")]
    [DataType(DataType.Text)]
    [Display(Name = "Session Name", Prompt = "Enter a Session Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Start time is required")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "End time is required")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
    [Display(Name = "End Time")]
    public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);

    [Required(ErrorMessage = "Available spots is required")]
    [Range(1, 50, ErrorMessage = "Must be between 1 and 50 spots available")]
    [Display(Name = "Available spots")]
    public int AvailableSpots { get; set; } = 1;

    public string? ErrorMessage { get; set; }
}

