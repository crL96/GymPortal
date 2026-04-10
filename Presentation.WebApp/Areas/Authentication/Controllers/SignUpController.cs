using Application.Abstractions.Auth;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Authentication.Models;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("registration")]
public class SignUpController(IAuthService authService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SetEmailForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        if (await authService.UserExists(form.Email))
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Email address is already registered, try signing in instead.");
            return View(form);
        }

        HttpContext.Session.SetString("SessionEmailAddress", form.Email);

        //Redirect to SetPassword view
        return View();
    }
}
