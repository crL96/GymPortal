namespace Presentation.WebApp.Areas.Account.Models;

public class BookingPageViewModel
{
    public List<TrainingSession> Sessions { get; set; } = [];
    public List<UpcomingBooking> UpcomingBookings { get; set; } = [];

}
