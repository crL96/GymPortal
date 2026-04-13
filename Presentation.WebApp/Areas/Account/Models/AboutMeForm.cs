using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Account.Models;

public class AboutMeForm
{
    [Required(ErrorMessage = "First name is required")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name", Prompt = "Enter First Name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name", Prompt = "Enter Last Name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "You must enter an email address")]
    [EmailAddress(ErrorMessage = "You must enter a valid email address")]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;

    [Phone(ErrorMessage = "Phone number must be valid")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number", Prompt = "Enter Phone Number")]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Profile Image", Prompt = "Select Profile Image")]
    public IFormFile? ProfileImage { get; set; }
}
