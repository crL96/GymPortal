using Application.Abstractions.Repositories.Bookings;
using Application.Abstractions.Repositories.Memberships;
using Application.Abstractions.Repositories.TrainingSessions;
using Application.Abstractions.Services.Bookings;
using Application.Dtos.Bookings;
using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.Exceptions;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;

namespace Application.Services;

public class BookingService(
    ITrainingSessionRepository sessionRepo,
    IUserMembershipRepository userMembershipRepo,
    IBookingRepository bookingRepo)
    : IBookingService
{
    public async Task<BookingResult> CreateBooking(Guid sessionId, string userId, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BookingResult.Failed("Invalid UserId");

            var trainingSessionId = TrainingSessionId.Recreate(sessionId);

            var session = await sessionRepo.GetByIdAsync(trainingSessionId);
            if (session is null)
                return BookingResult.NotFound("Session not found");

            var sessionBookings = await bookingRepo.GetBySessionId(trainingSessionId, ct);
            var bookedUserIds = sessionBookings.Select(x => x.UserId).ToList();

            var userMembership = await userMembershipRepo.GetByIdAsync(userId, ct);

            var booking = BookingPolicy.CreateBooking(session, userMembership, bookedUserIds);

            var created = bookingRepo.CreateAsync(booking, ct);
            return created is not null ?
                BookingResult.Ok() :
                BookingResult.Failed("Failed to save booking to database");
        }
        catch (DoubleBookingDomainException)
        {
            return BookingResult.UserAlreadyBooked();
        }
        catch (FullSessionDomainException)
        {
            return BookingResult.SessionFull();
        }
        catch (InvalidMembershipDomainException)
        {
            return BookingResult.InvalidMembership();
        }
        catch (Exception ex)
        {
            return BookingResult.Failed(ex.Message);
        }
    }

    public async Task<BookingListResult> GetUserUpcomingBookings(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BookingListResult.Failed("Invalid UserId");

        var bookings = await bookingRepo.GetByUserId(userId, DateTime.Now, DateTime.Now.AddYears(1), ct);
        return BookingListResult.Ok(bookings.ToList());
    }

    public async Task<BookingResult> RemoveBooking(Guid bookingId, CancellationToken ct = default)
    {
        var success = await bookingRepo.DeleteAsync(BookingId.Recreate(bookingId), ct);
        return success ?
            BookingResult.Ok() :
            BookingResult.Failed("Failed to remove booking");
    }

    public async Task<BookingResult> RemoveBookingByUserAndSession(Guid sessionId, string userId, CancellationToken ct = default)
    {
        var success = await bookingRepo.DeleteByUserAndSessionIdAsync(userId, TrainingSessionId.Recreate(sessionId), ct);
        return success ?
            BookingResult.Ok() :
            BookingResult.Failed("Failed to remove booking");
    }
}
