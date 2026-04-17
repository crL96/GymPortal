using Application.Dtos.TrainingSessions;
using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.TrainingSessions.ValueObjects;

namespace Application.Abstractions.Repositories.TrainingSessions;

public interface ITrainingSessionRepository : IRepositoryBase<TrainingSession, TrainingSessionId>
{
    Task<IReadOnlyList<TrainingSessionDto>> GetByTimePeriodWithBookings(DateTime startTime, DateTime endTime, CancellationToken ct = default);
}
