using Application.Abstractions.Services.CustomerService;
using Application.Dtos.CustomerService;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Support;

namespace Presentation.WebApp.Controllers;

public class SupportController(IContactService contactService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var dto = new ContactRequest(null,
            form.FirstName,
            form.LastName,
            form.Email,
            form.Phone,
            form.Message,
            form.AcceptSavePersonalInformation,
            DateTime.UtcNow
        );
        var result = await contactService.SaveContactRequest(dto);
        if (!result.Succeeded)
            TempData["FormSubmitMessage"] = "Could not submit form, please try again later.";
        else
            TempData["FormSubmitMessage"] = "We have received your message and will get back to you.";

        return View();
    }
}
