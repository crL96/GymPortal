using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Authentication.Models;

public class SetEmailForm
{
    [Required(ErrorMessage = "You must enter an email address")]
    [EmailAddress(ErrorMessage = "You must enter a valid email address")]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;


    [Range(typeof(bool), "true", "true", ErrorMessage = "Accepting the user terms & conditions is required")]
    [Display(Name = "I accept the User Terms & Conditions")]
    public bool TermsAndConditions { get; set; }


    public string? ErrorMessage { get; set; }
}
