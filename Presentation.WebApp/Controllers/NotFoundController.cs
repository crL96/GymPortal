using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

[Route("404")]
public class NotFoundController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

}
