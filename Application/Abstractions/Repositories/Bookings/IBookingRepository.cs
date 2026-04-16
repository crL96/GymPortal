using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;

namespace Application.Abstractions.Repositories.Bookings;

public interface IBookingRepository : IRepositoryBase<Booking, BookingId>
{
    Task<IReadOnlyList<Booking>> GetByUserId(string userId, DateTime startTime, DateTime endTime, CancellationToken ct = default);
}
