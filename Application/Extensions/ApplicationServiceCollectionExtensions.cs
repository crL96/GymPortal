using Application.Abstractions.Services.Bookings;
using Application.Abstractions.Services.Faq;
using Application.Abstractions.Services.Memberships;
using Application.Abstractions.Services.TrainingSessions;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(env);

        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IUserMembershipService, UserMembershipService>();
        services.AddScoped<ITrainingSessionService, TrainingSessionService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IFaqService, FaqService>();

        return services;
    }
}
