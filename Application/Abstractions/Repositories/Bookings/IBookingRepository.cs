using Application.Dtos.Bookings;
using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;

namespace Application.Abstractions.Repositories.Bookings;

public interface IBookingRepository : IRepositoryBase<Booking, BookingId>
{
    Task<IReadOnlyList<BookingDto>> GetByUserId(string userId, DateTime startTime, DateTime endTime, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> GetBySessionId(TrainingSessionId sessionId, CancellationToken ct = default);
    Task<bool> DeleteByUserAndSessionIdAsync(string userId, TrainingSessionId sessionId, CancellationToken ct = default);
}
