using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Support;

namespace Presentation.WebApp.Controllers;

public class SupportController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        //TODO - handle the form submit

        return View();
    }
}
