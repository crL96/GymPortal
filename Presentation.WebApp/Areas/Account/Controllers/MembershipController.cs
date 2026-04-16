using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Areas.Account.Controllers;

[Area("Account")]
[Authorize]
public class MembershipController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}