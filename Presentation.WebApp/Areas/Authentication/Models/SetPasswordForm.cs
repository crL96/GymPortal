using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Areas.Authentication.Models;

public class SetPasswordForm
{
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Length(minimumLength: 8, maximumLength: 30, ErrorMessage = "Password must be between 8-30 characters.")]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;


    [Required(ErrorMessage = "Password needs to be confirmed.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
    public string ConfirmPassword { get; set; } = null!;


    public string? ErrorMessage { get; set; }
}
