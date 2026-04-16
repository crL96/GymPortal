using Application.Abstractions.Repositories.TrainingSessions;
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

    public async Task<IReadOnlyList<TrainingSession>> GetByTimePeriod(DateTime startTime, DateTime endTime, CancellationToken ct)
    {
        var sessionEntities = await Set
            .Where(x => x.StartTime < endTime && x.EndTime > startTime)
            .AsNoTracking()
            .ToListAsync(ct);

        return sessionEntities.Select(ToModel).ToList();
    }
}
