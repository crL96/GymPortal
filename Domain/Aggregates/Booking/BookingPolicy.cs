using Domain.Aggregates.Booking.Exceptions;
using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.UserMemberships;

namespace Domain.Aggregates.Booking;

public static class BookingPolicy
{
    public static Booking CreateBooking(TrainingSession session, UserMembership? userMembership, int nBookings)
    {
        if (userMembership is null || userMembership.IsActive)
            throw new InvalidMembershipDomainException();

        if (session.IsFull(nBookings))
            throw new FullSessionDomainException();

        return Booking.Create(userMembership.UserId, session.Id);
    }
}
