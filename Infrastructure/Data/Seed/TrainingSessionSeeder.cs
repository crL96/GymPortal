using Application.Abstractions.Repositories.TrainingSessions;
using Domain.Aggregates.TrainingSessions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Seed;

public class TrainingSessionSeeder
{
    public static async Task SeedDevSessions(IServiceProvider sp)
    {
        await using var scope = sp.CreateAsyncScope();
        var repo = scope.ServiceProvider.GetRequiredService<ITrainingSessionRepository>();

        var sessions = new List<TrainingSession>()
        {
            TrainingSession.Create("Spinning with Sandra", DateTime.Now.Date.AddHours(12), DateTime.Now.Date.AddHours(13), 20),
            TrainingSession.Create("Gym introduction with Brian", DateTime.Now.Date.AddHours(15), DateTime.Now.Date.AddHours(16), 8),
            TrainingSession.Create("BodyPump with John", DateTime.Now.Date.AddHours(18), DateTime.Now.Date.AddHours(19), 25),

            TrainingSession.Create("Spinning with Sandra", DateTime.Now.Date.AddDays(1).AddHours(12), DateTime.Now.Date.AddDays(1).AddHours(13), 20),
            TrainingSession.Create("BodyPump with John", DateTime.Now.Date.AddDays(1).AddHours(18), DateTime.Now.Date.AddDays(1).AddHours(19), 25),
        };

        foreach (var session in sessions)
        {
            await repo.CreateAsync(session);
        }
    }
}
