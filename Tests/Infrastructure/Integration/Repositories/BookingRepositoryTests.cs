using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Repositories.Bookings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Tests.Infrastructure.Integration.Repositories;


[Collection("Database")]
public class BookingRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly BookingRepository _repo;
    private readonly ServiceProvider _provider;

    public BookingRepositoryTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _repo = new BookingRepository(_context);
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
            AvailableSpots = 10
        };
    }

    private async Task CleanDb()
    {
        await _context.TrainingSessions.ExecuteDeleteAsync();
        await _context.Users.ExecuteDeleteAsync();
    }

    [Fact]
    public async Task DeleteByUserAndSessionId_Should_Return_True_When_Entity_Exists()
    {
        // ARRANGE
        await CleanDb();
        var user = CreateTestUser();
        var session = CreateTestSession();

        var userManager = _provider.GetRequiredService<UserManager<AppUser>>();
        await userManager.CreateAsync(user);

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var booking = Booking.Recreate(BookingId.Create(), user.Id, session.Id);
        await _repo.CreateAsync(booking);
        await _context.SaveChangesAsync();

        // ACT
        var result = await _repo.DeleteByUserAndSessionIdAsync(user.Id, session.Id);
        await _context.SaveChangesAsync();

        // ASSERT
        Assert.True(result);
        Assert.Empty(_context.Bookings);
    }

    [Fact]
    public async Task DeleteByUserAndSessionId_Should_Return_False_When_Not_Found()
    {
        await CleanDb();
        var result = await _repo.DeleteByUserAndSessionIdAsync("missing-user", TrainingSessionId.Create());

        Assert.False(result);
    }


    [Fact]
    public async Task GetBySessionId_Should_Return_Bookings()
    {
        // ARRANGE
        await CleanDb();
        var user = CreateTestUser();
        var session = CreateTestSession();

        var userManager = _provider.GetRequiredService<UserManager<AppUser>>();
        await userManager.CreateAsync(user);

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var booking = Booking.Recreate(BookingId.Create(), user.Id, session.Id);

        await _repo.CreateAsync(booking);
        await _context.SaveChangesAsync();

        // ACT
        var result = await _repo.GetBySessionId(session.Id);

        // ASSERT
        Assert.Single(result);
    }

    [Fact]
    public async Task GetBySessionId_Should_Return_Empty_When_No_Bookings()
    {
        await CleanDb();
        var session = CreateTestSession();

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var result = await _repo.GetBySessionId(session.Id);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByUserId_Should_Return_Bookings_In_TimeRange()
    {
        // ARRANGE
        await CleanDb();
        var user = CreateTestUser();
        var session = CreateTestSession();

        var userManager = _provider.GetRequiredService<UserManager<AppUser>>();
        await userManager.CreateAsync(user);

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var booking = Booking.Recreate(BookingId.Create(), user.Id, session.Id);

        await _repo.CreateAsync(booking);
        await _context.SaveChangesAsync();

        // ACT
        var result = await _repo.GetByUserId(
            user.Id,
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(2));

        // ASSERT
        Assert.Single(result);
        Assert.Equal(user.Id, result[0].UserId);
    }

    [Fact]
    public async Task GetByUserId_Should_Return_Empty_When_Outside_TimeRange()
    {
        await CleanDb();
        var user = CreateTestUser();
        var session = CreateTestSession();

        var userManager = _provider.GetRequiredService<UserManager<AppUser>>();
        await userManager.CreateAsync(user);

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var booking = Booking.Recreate(BookingId.Create(), user.Id, session.Id);

        await _repo.CreateAsync(booking);
        await _context.SaveChangesAsync();

        var result = await _repo.GetByUserId(
            user.Id,
            DateTime.UtcNow.AddYears(-2),
            DateTime.UtcNow.AddYears(-1));

        Assert.Empty(result);
    }
}