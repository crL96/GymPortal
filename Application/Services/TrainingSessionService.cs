using Application.Abstractions.Repositories.TrainingSessions;
using Application.Abstractions.Services.TrainingSessions;
using Application.Dtos.TrainingSessions;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Common.Exceptions;

namespace Application.Services;

public class TrainingSessionService(ITrainingSessionRepository sessionRepo) : ITrainingSessionService
{
    public async Task<DeleteSessionResult> DeleteSessionAsync(Guid sessionId, string role, CancellationToken ct = default)
    {
        try
        {
            if (role != "Admin")
                return DeleteSessionResult.Unauthorized();

            var deleted = await sessionRepo.DeleteAsync(TrainingSessionId.Recreate(sessionId), ct);
            if (!deleted)
                return DeleteSessionResult.Failed("Failed to delete session");

            return DeleteSessionResult.Ok();
        }
        catch (InvalidIdDomainException)
        {
            return DeleteSessionResult.InvalidId();
        }

    }

    public async Task<TrainingSessionListResult> GetByTimePeriodWithBookings(DateTime startTime, DateTime endTime, CancellationToken ct = default)
    {
        try
        {
            var sessions = await sessionRepo.GetByTimePeriodWithBookings(startTime, endTime, ct);
            return TrainingSessionListResult.Ok(sessions.ToList());
        }
        catch (Exception ex)
        {
            return TrainingSessionListResult.Failed(ex.Message ?? "Failed to fetch sessions");
        }
    }
}
