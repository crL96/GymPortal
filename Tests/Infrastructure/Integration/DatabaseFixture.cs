using Infrastructure.Identity;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Infrastructure.Integration
{
    public class DatabaseFixture : IDisposable
    {
        private readonly SqliteConnection _connection;

        public ServiceProvider Provider { get; private set; } = default!;
        public ApplicationDbContext Context { get; }

        public DatabaseFixture()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddLogging();

            Provider = services.BuildServiceProvider();

            Context = Provider.GetRequiredService<ApplicationDbContext>();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Dispose();
            Provider.Dispose();
            _connection.Dispose();
        }
    }
}