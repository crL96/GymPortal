using Application.Abstractions.Services.TrainingSessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Admin.Models;
using Presentation.WebApp.Models.Common;

namespace Presentation.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SessionController(ITrainingSessionService sessionService) : Controller
{
    public async Task<IActionResult> IndexAsync(SessionPageViewModel viewModel, DateTime sessionSearchDate = default)
    {
        if (sessionSearchDate == default)
            sessionSearchDate = DateTime.Now.Date;

        var sessionsResult = await sessionService.GetByTimePeriodWithBookings(sessionSearchDate, sessionSearchDate.AddHours(24));
        if (!sessionsResult.Succeeded || sessionsResult.Sessions is null)
        {
            TempData["SessionBookerError"] = "Failed to fetch sessions, try again later";
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

}