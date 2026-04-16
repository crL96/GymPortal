using Application.Abstractions.Repositories.TrainingSessions;
using Application.Dtos.TrainingSessions;
using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.TrainingSessions;

public class TrainingSessionRepository(ApplicationDbContext context) :
    RepositoryBase<TrainingSession, TrainingSessionId, TrainingSessionEntity, ApplicationDbContext>(context),
    ITrainingSessionRepository
{
    protected override TrainingSessionEntity ToEntity(TrainingSession model)
    {
        return new TrainingSessionEntity()
        {
            Id = model.Id,
            Name = model.Name,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            AvailableSpots = model.AvailableSpots
        };
    }

    protected override TrainingSession ToModel(TrainingSessionEntity entity)
    {
        return TrainingSession.Recreate(
            entity.Id,
            entity.Name,
            entity.StartTime,
            entity.EndTime,
            entity.AvailableSpots
        );
    }

    public async Task<IReadOnlyList<TrainingSessionDto>> GetByTimePeriodWithBookings(DateTime startTime, DateTime endTime, CancellationToken ct)
    {
        var sessionEntities = await Set
            .Where(x => x.StartTime < endTime && x.EndTime > startTime)
            .Include(x => x.Bookings)
            .AsNoTracking()
            .ToListAsync(ct);

        var sessions = new List<TrainingSessionDto>();
        foreach (var entity in sessionEntities)
        {
            sessions.Add(new(
                entity.Id.Value,
                entity.Name,
                entity.StartTime,
                entity.EndTime,
                entity.AvailableSpots,
                entity.Bookings.Select(x => x.UserId).ToList()
            ));
        }

        return sessions;
    }
}
