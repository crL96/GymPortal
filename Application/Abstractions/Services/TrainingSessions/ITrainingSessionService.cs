using Application.Dtos.TrainingSessions;

namespace Application.Abstractions.Services.TrainingSessions;

public interface ITrainingSessionService
{
    Task<TrainingSessionListResult> GetByTimePeriodWithBookings(DateTime startTime, DateTime endTime, CancellationToken ct = default);
    Task<DeleteSessionResult> DeleteSessionAsync(Guid sessionId, string userId, CancellationToken ct = default);
    Task<CreateSessionResult> CreateSessionAsync(CreateSessionDto dto, string userId, CancellationToken ct = default);
}
