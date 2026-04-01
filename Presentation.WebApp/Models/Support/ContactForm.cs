using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Support;

public class ContactForm
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name *", Prompt = "Enter first name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name *", Prompt = "Enter last name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address *", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;

    [Phone(ErrorMessage = "Invalid phone number format")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number", Prompt = "ex. 070-123 45 67")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Message is required")]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Message *", Prompt = "Message...")]
    public string Message { get; set; } = null!;

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must allow us to save personal information in order to contact you")]
    [Display(Name = "I accept that CoreFitness saves my information *")]
    public bool AcceptSavePersonalInformation { get; set; }
}
