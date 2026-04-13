using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Areas.Account.Controllers;

[Area("account")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
