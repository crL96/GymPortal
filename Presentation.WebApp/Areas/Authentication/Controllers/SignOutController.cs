using Application.Abstractions.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Areas.Authentication.Controllers;

[Area("Authentication")]
[Route("sign-out")]
public class SignOutController(IAuthService authService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await authService.SignOutUserAsync();
        return Redirect("/");
    }

}