using Application.Abstractions.Repositories.Bookings;
using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Bookings;

public class BookingRepository(ApplicationDbContext context) :
    RepositoryBase<Booking, BookingId, BookingEntity, ApplicationDbContext>(context),
    IBookingRepository
{
    public async Task<IReadOnlyList<Booking>> GetByUserId(string userId, DateTime startTime, DateTime endTime, CancellationToken ct = default)
    {
        var bookingEntities = await Set
            .Where(x => x.UserId == userId)
            .Where(x => x.TrainingSession.StartTime < endTime && x.TrainingSession.EndTime > startTime)
            .AsNoTracking()
            .ToListAsync(ct);

        return bookingEntities.Select(ToModel).ToList();
    }

    protected override BookingEntity ToEntity(Booking model)
    {
        return new BookingEntity()
        {
            Id = model.Id,
            UserId = model.UserId,
            TrainingSessionId = model.TrainingSessionId
        };
    }

    protected override Booking ToModel(BookingEntity entity)
    {
        return Booking.Recreate(entity.Id, entity.UserId, entity.TrainingSessionId);
    }
}
