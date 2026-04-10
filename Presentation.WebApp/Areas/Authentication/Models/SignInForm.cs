using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Authentication.Models;

public class SignInForm
{
    [Required(ErrorMessage = "You must enter an email address")]
    [EmailAddress(ErrorMessage = "You must enter a valid email address")]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "You must enter a password")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; } = false;


    public string? ErrorMessage { get; set; }
}
