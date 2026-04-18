using Application.Abstractions.Repositories.TrainingSessions;
using Application.Abstractions.Services.Auth;
using Application.Abstractions.Services.TrainingSessions;
using Application.Dtos.TrainingSessions;
using Domain.Aggregates.TrainingSessions;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Domain.Common.Exceptions;

namespace Application.Services;

public class TrainingSessionService(ITrainingSessionRepository sessionRepo, IAuthService authService) : ITrainingSessionService
{
    public async Task<CreateSessionResult> CreateSessionAsync(CreateSessionDto dto, string userId, CancellationToken ct = default)
    {
        try
        {
            if (!await authService.IsUserAdmin(userId))
                return CreateSessionResult.Failed("User is not an admin");

            var session = TrainingSession.Create(dto.Name, dto.StartTime, dto.EndTime, dto.AvailableSpots);

            var created = await sessionRepo.CreateAsync(session, ct);
            if (created is null)
                return CreateSessionResult.Failed("Failed to save session to database.");

            return CreateSessionResult.Ok();
        }
        catch (DomainException ex)
        {
            return CreateSessionResult.Failed(ex.Message);
        }

    }

    public async Task<DeleteSessionResult> DeleteSessionAsync(Guid sessionId, string userId, CancellationToken ct = default)
    {
        try
        {
            if (!await authService.IsUserAdmin(userId))
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
