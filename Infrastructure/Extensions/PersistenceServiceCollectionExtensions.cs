using Application.Abstractions.Repositories.Bookings;
using Application.Abstractions.Repositories.Faq;
using Application.Abstractions.Repositories.Memberships;
using Application.Abstractions.Repositories.TrainingSessions;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.Bookings;
using Infrastructure.Persistence.Repositories.Faq;
using Infrastructure.Persistence.Repositories.Memberships;
using Infrastructure.Persistence.Repositories.TrainingSessions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Extensions;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(env);

        if (env.IsDevelopment())
        {
            Console.WriteLine("Using development database");

            services.AddSingleton<SqliteConnection>(_ =>
            {
                var conn = new SqliteConnection(configuration.GetConnectionString("DevDbConnection") ?? "Data Source=:memory:;");
                conn.Open();
                return conn;
            });

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var conn = sp.GetRequiredService<SqliteConnection>();
                options.UseSqlite(conn);
            });
        }
        else
        {
            Console.WriteLine("Using production database");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        }

        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IUserMembershipRepository, UserMembershipRepository>();
        services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IFaqRepository, FaqRepository>();

        return services;
    }
}