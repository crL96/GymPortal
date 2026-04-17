using Infrastructure.Identity;
using Infrastructure.Persistence.Configurations;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;

public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<MembershipEntity> Memberships => Set<MembershipEntity>();
    public DbSet<UserMembershipEntity> UserMembership => Set<UserMembershipEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
    public DbSet<TrainingSessionEntity> TrainingSessions => Set<TrainingSessionEntity>();
    public DbSet<FaqEntity> Faqs => Set<FaqEntity>();
    public DbSet<ContactRequestEntity> ContactRequests => Set<ContactRequestEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new MembershipEntityConfiguration());
        builder.ApplyConfiguration(new UserMembershipEntityConfiguration());
        builder.ApplyConfiguration(new BookingEntityConfiguration());
        builder.ApplyConfiguration(new TrainingSessionEntityConfiguration());
        builder.ApplyConfiguration(new FaqEntityConfiguration());
        builder.ApplyConfiguration(new ContactRequestEntityConfiguration());
    }
}
