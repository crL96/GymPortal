using Application.Abstractions.Repositories.Bookings;
using Application.Dtos.Bookings;
using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Bookings;

public class BookingRepository(ApplicationDbContext context) :
    RepositoryBase<Booking, BookingId, BookingEntity, ApplicationDbContext>(context),
    IBookingRepository
{
    public async Task<bool> DeleteByUserAndSessionIdAsync(string userId, TrainingSessionId sessionId, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FirstOrDefaultAsync(x => x.TrainingSessionId == sessionId && x.UserId == userId, ct);
            if (entity == null)
                return false;

            Set.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IReadOnlyList<Booking>> GetBySessionId(TrainingSessionId sessionId, CancellationToken ct = default)
    {
        var entities = await Set
            .Where(x => x.TrainingSessionId == sessionId)
            .AsNoTracking()
            .ToListAsync(ct);

        return entities.Select(ToModel).ToList();
    }

    public async Task<IReadOnlyList<BookingDto>> GetByUserId(string userId, DateTime startTime, DateTime endTime, CancellationToken ct = default)
    {
        var bookingEntities = await Set
            .Where(x => x.UserId == userId)
            .Where(x => x.TrainingSession.StartTime < endTime && x.TrainingSession.EndTime > startTime)
            .Include(x => x.TrainingSession)
            .AsNoTracking()
            .ToListAsync(ct);

        var bookings = bookingEntities
            .Select(b => BookingDto.Create(
                b.Id.Value,
                b.UserId,
                b.TrainingSessionId.Value,
                b.TrainingSession.Name,
                b.TrainingSession.StartTime,
                b.TrainingSession.EndTime
            )).ToList();

        return bookings;
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
