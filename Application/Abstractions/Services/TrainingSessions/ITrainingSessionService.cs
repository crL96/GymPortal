using Application.Dtos.TrainingSessions;

namespace Application.Abstractions.Services.TrainingSessions;

public interface ITrainingSessionService
{
    Task<TrainingSessionListResult> GetByTimePeriodWithBookings(DateTime startTime, DateTime endTime, CancellationToken ct = default);
}
