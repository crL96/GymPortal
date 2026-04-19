using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.Bookings;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration.Repositories;

[Collection("Database")]
public class BaseRepositoryBehaviorTests
{
    private readonly ApplicationDbContext _context;
    private readonly BookingRepository _repo;

    public BaseRepositoryBehaviorTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _repo = new BookingRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_Should_Persist_Entity()
    {
        var id = BookingId.Create();
        var booking = Booking.Recreate(id, "user-1", TrainingSessionId.Create());

        await _repo.CreateAsync(booking);

        var saved = await _repo.GetByIdAsync(id);

        Assert.Equal(booking.Id, saved!.Id);
        Assert.Equal(booking.UserId, saved!.UserId);
        Assert.Equal(booking.TrainingSessionId, saved!.TrainingSessionId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity_When_Found()
    {
        var booking = Booking.Recreate(BookingId.Create(), "user-1", TrainingSessionId.Create());
        await _repo.CreateAsync(booking);

        var result = await _repo.GetByIdAsync(booking.Id);

        Assert.NotNull(result);
        Assert.Equal("user-1", result!.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var result = await _repo.GetByIdAsync(BookingId.Create());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Entity()
    {
        var booking = Booking.Recreate(BookingId.Create(), "user-1", TrainingSessionId.Create());
        await _repo.CreateAsync(booking);

        var updated = Booking.Recreate(booking.Id, "user-2", booking.TrainingSessionId);

        await _repo.UpdateAsync(booking.Id, updated);

        var result = await _repo.GetByIdAsync(booking.Id);

        Assert.Equal("user-2", result!.UserId);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Entity()
    {
        var booking = Booking.Recreate(BookingId.Create(), "user-1", TrainingSessionId.Create());
        await _repo.CreateAsync(booking);

        await _repo.DeleteAsync(booking.Id);

        var result = await _repo.GetByIdAsync(booking.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_Return_False_When_Entity_Does_Not_Exist()
    {
        var result = await _repo.DeleteAsync(BookingId.Create());

        Assert.False(result);
    }
}
