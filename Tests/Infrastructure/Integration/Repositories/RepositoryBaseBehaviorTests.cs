using Domain.Aggregates.Booking;
using Domain.Aggregates.Booking.ValueObjects;
using Domain.Aggregates.TrainingSessions.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.Bookings;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration.Repositories;

public class BaseRepositoryBehaviorTests
{
    private ApplicationDbContext CreateContext()
    {
        var provider = TestDbFactory.CreateProvider();
        return provider.GetRequiredService<ApplicationDbContext>();
    }

    private BookingRepository CreateRepo()
    {
        var context = CreateContext();
        return new BookingRepository(context);
    }

    [Fact]
    public async Task CreateAsync_Should_Persist_Entity()
    {
        var repo = CreateRepo();

        var id = BookingId.Create();
        var booking = Booking.Recreate(id, "user-1", TrainingSessionId.Create());

        await repo.CreateAsync(booking);

        var saved = await repo.GetByIdAsync(id);

        Assert.Equal(booking.Id, saved!.Id);
        Assert.Equal(booking.UserId, saved!.UserId);
        Assert.Equal(booking.TrainingSessionId, saved!.TrainingSessionId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity_When_Found()
    {
        var repo = CreateRepo();

        var booking = Booking.Recreate(BookingId.Create(), "user-1", TrainingSessionId.Create());
        await repo.CreateAsync(booking);

        var result = await repo.GetByIdAsync(booking.Id);

        Assert.NotNull(result);
        Assert.Equal("user-1", result!.UserId);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var repo = CreateRepo();

        var result = await repo.GetByIdAsync(BookingId.Create());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Entity()
    {
        var repo = CreateRepo();

        var booking = Booking.Recreate(BookingId.Create(), "user-1", TrainingSessionId.Create());
        await repo.CreateAsync(booking);

        var updated = Booking.Recreate(booking.Id, "user-2", booking.TrainingSessionId);

        await repo.UpdateAsync(booking.Id, updated);

        var result = await repo.GetByIdAsync(booking.Id);

        Assert.Equal("user-2", result!.UserId);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Entity()
    {
        var repo = CreateRepo();

        var booking = Booking.Recreate(BookingId.Create(), "user-1", TrainingSessionId.Create());
        await repo.CreateAsync(booking);

        await repo.DeleteAsync(booking.Id);

        var result = await repo.GetByIdAsync(booking.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_Return_False_When_Entity_Does_Not_Exist()
    {
        var repo = CreateRepo();

        var result = await repo.DeleteAsync(BookingId.Create());

        Assert.False(result);
    }
}
