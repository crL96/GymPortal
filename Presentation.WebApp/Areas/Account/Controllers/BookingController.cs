using System.Security.Claims;
using Application.Abstractions.Services.Bookings;
using Application.Abstractions.Services.TrainingSessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Account.Models;

namespace Presentation.WebApp.Areas.Account.Controllers;

[Area("Account")]
[Authorize]
public class BookingController(ITrainingSessionService sessionService, IBookingService bookingService) : Controller
{
    public async Task<IActionResult> Index(BookingPageViewModel viewModel, DateTime bookingSearchDate = default)
    {
        if (bookingSearchDate == default)
            bookingSearchDate = DateTime.Now.Date;

        var sessionsResult = await sessionService.GetByTimePeriodWithBookings(bookingSearchDate, bookingSearchDate.AddHours(24));
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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Redirect("sign-out");

        var bookingResult = await bookingService.GetUserUpcomingBookings(userId);
        if (!bookingResult.Succeeded || bookingResult.Bookings is null)
        {
            TempData["UpcomingBookingsError"] = "Failed to fetch upcoming bookings, try again later.";
            return View(viewModel);
        }

        viewModel.UpcomingBookings = bookingResult.Bookings
            .Select(x => new UpcomingBooking(
                x.Id,
                new(
                    x.TrainingSession.Id,
                    x.TrainingSession.Name,
                    x.TrainingSession.StartTime,
                    x.TrainingSession.EndTime
                )))
            .OrderBy(x => x.Session.StartTime)
            .ToList();

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBooking(Guid sessionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Redirect("sign-out");

        var result = await bookingService.CreateBooking(sessionId, userId);
        if (!result.Succeeded)
        {
            TempData["SessionBookerError"] = result.ErrorMessage ?? "Could not create booking";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(Guid sessionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Redirect("sign-out");


        var result = await bookingService.RemoveBookingByUserAndSession(sessionId, userId);
        if (!result.Succeeded)
        {
            TempData["SessionBookerError"] = result.ErrorMessage ?? "Could not create booking";
        }
        return RedirectToAction(nameof(Index));
    }
}