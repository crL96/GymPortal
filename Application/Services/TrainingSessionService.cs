using Application.Abstractions.Repositories.TrainingSessions;
using Application.Abstractions.Services.TrainingSessions;
using Application.Dtos.TrainingSessions;

namespace Application.Services;

public class TrainingSessionService(ITrainingSessionRepository sessionRepo) : ITrainingSessionService
{
    public async Task<TrainingSessionListResult> GetByTimePeriodWithBookings(DateTime startTime, DateTime endTime, CancellationToken ct = default)
    {
        try
        {
            var sessions = await sessionRepo.GetByTimePeriodWithBookings(startTime, endTime, ct);
            return TrainingSessionListResult.Ok((List<TrainingSessionDto>)sessions);
        }
        catch (Exception ex)
        {
            return TrainingSessionListResult.Failed(ex.Message ?? "Failed to fetch sessions");
        }
    }
}
