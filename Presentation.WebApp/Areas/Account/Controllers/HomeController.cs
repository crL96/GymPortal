using System.Security.Claims;
using Application.Abstractions.User;
using Application.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Account.Models;

namespace Presentation.WebApp.Areas.Account.Controllers;

[Area("account")]
[Authorize]
public class HomeController(IUserService userService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Redirect("/sign-out");
        }

        var account = await userService.GetUserDetailsAsync(userId);
        var viewModel = new AboutMeViewModel
        {
            AboutMeForm = new AboutMeForm
            {
                FirstName = account.UserDetails?.FirstName ?? "",
                LastName = account.UserDetails?.LastName ?? "",
                Email = account.UserDetails?.Email ?? "",
                PhoneNumber = account.UserDetails?.PhoneNumber ?? ""
            },
            ProfileImageUrl = account.UserDetails?.ImageUrl,
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(AboutMeViewModel viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Redirect("/sign-out");

        var account = await userService.GetUserDetailsAsync(userId);
        if (account.UserDetails is null)
            return Redirect("/sign-out");

        viewModel.ProfileImageUrl = account.UserDetails.ImageUrl;

        if (!ModelState.IsValid)
            return View(viewModel);

        var imageUrl = account.UserDetails.ImageUrl;

        if (viewModel.AboutMeForm.ProfileImage is not null && viewModel.AboutMeForm.ProfileImage.Length > 0)
        {
            imageUrl = await SaveProfileImageAsync(viewModel.AboutMeForm.ProfileImage);
        }

        var details = new UserDetails(
            userId,
            viewModel.AboutMeForm.Email,
            viewModel.AboutMeForm.FirstName,
            viewModel.AboutMeForm.LastName,
            viewModel.AboutMeForm.PhoneNumber,
            imageUrl
        );

        var result = await userService.UpdateUserDetailsAsync(details);
        if (!result.Succeeded)
        {
            viewModel.ProfileImageUrl = imageUrl;
            viewModel.ErrorMessage = "Something went wrong, please try again";
            return View(viewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> RemoveAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Redirect("/sign-out");

        var deleted = await userService.DeleteUserAsync(userId);
        if (!deleted.Succeeded)
        {
            TempData["Message"] = "Something went wrong, account was NOT deleted.";
            return RedirectToAction(nameof(Index));
        }

        return Redirect("/sign-out");
    }

    private static async Task<string> SaveProfileImageAsync(IFormFile file)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-images");
        Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/uploads/profile-images/{fileName}";
    }
}
