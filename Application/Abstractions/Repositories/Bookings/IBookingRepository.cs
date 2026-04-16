using Application.Dtos.Bookings;
using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;

namespace Application.Abstractions.Repositories.Bookings;

public interface IBookingRepository : IRepositoryBase<Booking, BookingId>
{
    Task<IReadOnlyList<BookingDto>> GetByUserId(string userId, DateTime startTime, DateTime endTime, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetBySessionId(Guid sessionId, CancellationToken ct = default);
}
