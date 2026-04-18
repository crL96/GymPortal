using Application.Abstractions.Services.Auth;
using Application.Dtos.Auth;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Authentication.Models;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
public class SignInController(IAuthService authService, SignInManager<AppUser> signInManager) : Controller
{
    [HttpGet("sign-in")]
    public IActionResult Index(string? returnUrl = null)
    {
        var redirect = GetRedirectIfSignedIn();
        if (redirect is not null)
            return redirect;

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost("sign-in")]
    [ValidateAntiForgeryToken]
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

            return GetRedirectIfSignedIn() ?? Redirect("/");
        }
        catch
        {
            ModelState.AddModelError(nameof(form.ErrorMessage), "Something went wrong, please try again");
            ViewBag.ReturnUrl = returnUrl;

            return View(form);
        }
    }

    [HttpPost("external-login")]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        var callbackUrl = Url.Action(nameof(ExternalLoginCallback), "SignIn", new
        {
            area = "Authentication",
            returnUrl
        });

        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, callbackUrl);
        return Challenge(properties, provider);
    }

    [HttpGet("external-login")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            TempData["ExternalErrorMessage"] = $"External provider error: {remoteError}";
            return RedirectToAction(nameof(Index), new { returnUrl });
        }

        var result = await authService.SignInExternalUserAsync("Member");
        if (!result.Succeeded)
        {
            TempData["ExternalErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index), new { returnUrl });
        }

        if (!string.IsNullOrWhiteSpace(returnUrl))
            return Redirect(returnUrl);

        return GetRedirectIfSignedIn() ?? Redirect("/");
    }


    private RedirectResult? GetRedirectIfSignedIn()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("Admin"))
                return Redirect("/admin");

            return Redirect("/account");
        }

        return null;
    }
}