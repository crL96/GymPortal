using Domain.Aggregates.Booking.Exceptions;
using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.UserMemberships;

namespace Domain.Aggregates.Booking;

public static class BookingPolicy
{
    public static Booking CreateBooking(TrainingSession session, UserMembership? userMembership, List<string> bookedUserIds)
    {
        if (userMembership is null || !userMembership.IsActive)
            throw new InvalidMembershipDomainException();

        if (session.IsFull(bookedUserIds.Count))
            throw new FullSessionDomainException();

        if (bookedUserIds.Contains(userMembership.UserId))
            throw new DoubleBookingDomainException();

        return Booking.Create(userMembership.UserId, session.Id);
    }
}
