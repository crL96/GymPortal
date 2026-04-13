using Application.Abstractions.Auth;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Authentication.Models;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("sign-in")]
public class SignInController(IAuthService authService) : Controller
{
    public IActionResult Index(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(SignInForm form, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid)
            return View(form);

        try
        {
            var signInResult = await authService.SignInUserAsync(form.Email, form.Password, form.RememberMe);

            if (!signInResult.Succeeded)
            {
                var errorMessage = signInResult.ErrorType switch
                {
                    AuthErrorType.RequiresTwoFactorAuth => "This user requires two-factor authentication",
                    AuthErrorType.IsLockedOut => "This user is locked out. Please contact support.",
                    AuthErrorType.NotAllowed => "You are not allowed to login. Please contact support.",
                    _ => "Incorrect email address or password"
                };

                ModelState.AddModelError(nameof(form.ErrorMessage), errorMessage);
                ViewBag.ReturnUrl = returnUrl;

                return View(form);
            }

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return Redirect("/account");
        }
        catch
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Something went wrong, please try again");
            ViewBag.ReturnUrl = returnUrl;

            return View(form);
        }
    }
}