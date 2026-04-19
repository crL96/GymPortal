using System;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Repositories.TrainingSessions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration.Repositories;

[Collection("Database")]
public class TrainingSessionRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly TrainingSessionRepository _repo;
    private readonly ServiceProvider _provider;

    public TrainingSessionRepositoryTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _repo = new TrainingSessionRepository(_context);
        _provider = fixture.Provider;
    }
    private AppUser CreateTestUser()
    {
        return new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "user@test.com",
            Email = "user@test.com"
        };
    }

    private TrainingSessionEntity CreateTestSession()
    {
        return new TrainingSessionEntity
        {
            Id = TrainingSessionId.Create(),
            Name = "Test",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            AvailableSpots = 10,
            Bookings = new List<BookingEntity>()
        };
    }

    private async Task CleanDb()
    {
        await _context.Bookings.ExecuteDeleteAsync();
        await _context.TrainingSessions.ExecuteDeleteAsync();
        await _context.Users.ExecuteDeleteAsync();
    }


    [Fact]
    public async Task GetByTimePeriodWithBookings_Should_Return_Sessions_With_Bookings()
    {
        // ARRANGE
        await CleanDb();

        var user = CreateTestUser();
        var userManager = _provider.GetRequiredService<UserManager<AppUser>>();
        await userManager.CreateAsync(user);

        var session = CreateTestSession();

        var booking = new BookingEntity
        {
            Id = BookingId.Create(),
            UserId = user.Id,
            TrainingSessionId = session.Id
        };

        session.Bookings.Add(booking);

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        // ACT
        var result = await _repo.GetByTimePeriodWithBookings(
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            CancellationToken.None);

        // ASSERT
        Assert.Single(result);
        Assert.Single(result[0].BookedIds);
        Assert.Equal(user.Id, result[0].BookedIds[0]);
    }

    [Fact]
    public async Task GetByTimePeriodWithBookings_Should_Return_Empty_When_No_Sessions()
    {
        await CleanDb();

        var result = await _repo.GetByTimePeriodWithBookings(
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByTimePeriodWithBookings_Should_Exclude_Sessions_Outside_Range()
    {
        // ARRANGE
        await CleanDb();
        var session = new TrainingSessionEntity
        {
            Id = TrainingSessionId.Create(),
            Name = "Outside",
            StartTime = DateTime.UtcNow.AddDays(10),
            EndTime = DateTime.UtcNow.AddDays(10).AddHours(1),
            AvailableSpots = 10
        };

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        // ACT
        var result = await _repo.GetByTimePeriodWithBookings(
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            CancellationToken.None);

        // ASSERT
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByTimePeriodWithBookings_Should_Return_Session_With_Empty_Bookings()
    {
        // ARRANGE
        var session = CreateTestSession();

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        // ACT
        var result = await _repo.GetByTimePeriodWithBookings(
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            CancellationToken.None);

        // ASSERT
        Assert.Single(result);
        Assert.Empty(result[0].BookedIds);
    }
}
