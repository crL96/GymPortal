using System.Security.Claims;
using Application.Abstractions.Services.TrainingSessions;
using Application.Dtos.TrainingSessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Account.Models;
using Presentation.WebApp.Areas.Admin.Models;
using Presentation.WebApp.Models.Common;

namespace Presentation.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SessionController(ITrainingSessionService sessionService) : Controller
{
    public async Task<IActionResult> Index(SessionPageViewModel viewModel, DateTime sessionSearchDate = default)
    {
        ModelState.Remove("CreateSessionForm.Name");
        if (sessionSearchDate == default)
            sessionSearchDate = DateTime.Now.Date;

        var sessionsResult = await sessionService.GetByTimePeriodWithBookings(sessionSearchDate, sessionSearchDate.AddHours(24));
        if (!sessionsResult.Succeeded || sessionsResult.Sessions is null)
        {
            TempData["SessionHandlerMessage"] = "Failed to fetch sessions, try again later";
            return View(viewModel);
        }

        viewModel.Sessions = sessionsResult.Sessions.Select(x => new TrainingSession()
        {
            Id = x.Id,
            Name = x.Name,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            AvailableSpots = x.AvailableSpots,
            BookedIds = x.BookedIds
        }).OrderBy(x => x.StartTime).ToList();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteSession(Guid sessionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Redirect("/sign-out");

        var result = await sessionService.DeleteSessionAsync(sessionId, userId);

        if (result.Succeeded)
        {
            TempData["SessionHandlerMessage"] = "Session deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        if (result.ErrorType == DeleteSessionErrorType.Unauthorized)
            return Redirect("/sign-out");

        if (result.ErrorType == DeleteSessionErrorType.InvalidId)
            TempData["SessionHandlerMessage"] = "Invalid session id, could not delete";

        TempData["SessionHandlerMessage"] = "Something went wrong, failed to delete";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession(SessionPageViewModel viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Redirect("/sign-out");

        if (viewModel.CreateSessionForm.StartTime < DateTime.Now)
            ModelState.AddModelError("CreateSessionForm.StartTime", "Start time cannot be in the past");

        if (viewModel.CreateSessionForm.EndTime < viewModel.CreateSessionForm.StartTime.AddMinutes(30))
            ModelState.AddModelError("CreateSessionForm.EndTime", "End time must be atleast 30 minutes after start.");

        if (!ModelState.IsValid)
            return View(nameof(Index), viewModel);

        var dto = CreateSessionDto.Create(
            viewModel.CreateSessionForm.Name,
            viewModel.CreateSessionForm.StartTime,
            viewModel.CreateSessionForm.EndTime,
            viewModel.CreateSessionForm.AvailableSpots
        );

        var result = await sessionService.CreateSessionAsync(dto, userId);
        if (!result.Succeeded)
        {
            TempData["create-session-message"] = result.ErrorMessage ?? "Something went wrong";
            return View(nameof(Index), viewModel);
        }

        TempData["create-session-message"] = "Session created successfully";
        return RedirectToAction(nameof(Index));
    }

}