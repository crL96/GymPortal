using Application.Abstractions.Auth;
using Application.Dtos.Auth;
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

        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet("set-password")]
    public IActionResult SetPassword()
    {
        var email = HttpContext.Session.GetString("SessionEmailAddress");
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(Index));

        return View();
    }

    [HttpPost("set-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var email = HttpContext.Session.GetString("SessionEmailAddress");
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(Index));

        var result = await authService.SignUpUserAsync(email, form.Password, "Member");
        if (!result.Succeeded)
        {
            if (result.ErrorType.Equals(AuthErrorType.UserAlreadyExists))
                ModelState.AddModelError(nameof(form.ErrorMessage), "User already exists");
            else
                ModelState.AddModelError(nameof(form.ErrorMessage), result?.ErrorMessage ?? "Something went wrong, please try again");

            return View(form);
        }

        await authService.SignInUserAsync(email, form.Password, false);

        return Redirect("/account");
    }
}
