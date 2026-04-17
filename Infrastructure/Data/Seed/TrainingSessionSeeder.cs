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
            TrainingSession.Create("Spinning with Sandra", DateTime.Now.AddHours(1), DateTime.Now.AddHours(2), 20),
            TrainingSession.Create("Gym introduction with Brian", DateTime.Now.AddHours(3), DateTime.Now.AddHours(4), 8),
            TrainingSession.Create("BodyPump with John", DateTime.Now.AddHours(7), DateTime.Now.AddHours(8), 25),

            TrainingSession.Create("Spinning with Sandra", DateTime.Now.AddDays(1).AddHours(1), DateTime.Now.AddDays(1).AddHours(2), 20),
            TrainingSession.Create("BodyPump with John", DateTime.Now.AddDays(1).AddHours(7), DateTime.Now.AddDays(1).AddHours(8), 25),
        };

        foreach (var session in sessions)
        {
            await repo.CreateAsync(session);
        }
    }
}
