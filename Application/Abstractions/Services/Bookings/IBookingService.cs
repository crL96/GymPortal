using Application.Dtos.Bookings;

namespace Application.Abstractions.Services.Bookings;

public interface IBookingService
{
    Task<BookingResult> CreateBooking(Guid sessionId, string userId, CancellationToken ct = default);
    Task<BookingResult> RemoveBooking(Guid bookingId, CancellationToken ct = default);
    Task<BookingResult> RemoveBookingByUserAndSession(Guid sessionId, string userId, CancellationToken ct = default);
    Task<BookingListResult> GetUserUpcomingBookings(string userId, CancellationToken ct = default);
}
