using Application.Abstractions.Services.TrainingSessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Account.Models;

namespace Presentation.WebApp.Areas.Account.Controllers;

[Area("Account")]
[Authorize]
public class BookingController(ITrainingSessionService sessionService) : Controller
{
    public async Task<IActionResult> Index(BookingPageViewModel viewModel, DateTime bookingSearchDate = default)
    {
        if (bookingSearchDate == default)
            bookingSearchDate = DateTime.Now.Date;

        var bookingsResult = await sessionService.GetByTimePeriodWithBookings(bookingSearchDate, bookingSearchDate.AddHours(24));
        if (!bookingsResult.Succeeded || bookingsResult.Sessions is null)
        {
            TempData["SessionBookerError"] = "Failed to fetch sessions, try again later";
            return View(viewModel);
        }

        viewModel.Sessions = bookingsResult.Sessions.Select(x => new TrainingSession()
        {
            Id = x.Id,
            Name = x.Name,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            AvailableSpots = x.AvailableSpots,
            BookedIds = x.BookedIds
        }).ToList();

        return View(viewModel);
    }

}