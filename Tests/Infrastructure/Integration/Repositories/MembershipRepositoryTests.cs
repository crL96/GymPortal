using Domain.Aggregates.Membership.ValueObjects;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Repositories.Bookings;
using Infrastructure.Persistence.Repositories.Memberships;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration.Repositories;

[Collection("Database")]
public class MembershipRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly MembershipRepository _repo;

    public MembershipRepositoryTests(DatabaseFixture fixture)
    {
        _context = fixture.Context;
        _repo = new MembershipRepository(_context);
    }

    [Fact]
    public async Task GetByName_Should_Return_Membership_When_Found()
    {
        // ARRANGE
        var membership = new MembershipEntity
        {
            Id = MembershipId.Create(),
            Name = "Standard",
            Price = 100
        };

        _context.Memberships.Add(membership);
        await _context.SaveChangesAsync();

        // ACT
        var result = await _repo.GetByName("Standard");

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("Standard", result!.Name);
    }

    [Fact]
    public async Task GetByName_Should_Return_Null_When_NotFound()
    {
        var result = await _repo.GetByName("DoesNotExist");

        Assert.Null(result);
    }
}
