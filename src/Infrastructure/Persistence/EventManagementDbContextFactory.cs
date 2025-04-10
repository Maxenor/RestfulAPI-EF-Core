using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Factory for creating instances of the EventManagementDbContext.
    /// This is used during design time, such as when running migrations.
    /// </summary>
    public class EventManagementDbContextFactory : IDesignTimeDbContextFactory<EventManagementDbContext>
    {
        public EventManagementDbContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json and environment variables
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("MariaDbConnection");

            var serverVersion = new MariaDbServerVersion(new Version(10, 6, 0));

            var optionsBuilder = new DbContextOptionsBuilder<EventManagementDbContext>();
            optionsBuilder.UseMySql(connectionString, serverVersion,
                mySqlOptions => mySqlOptions.MigrationsAssembly(typeof(EventManagementDbContextFactory).Assembly.FullName)
            );

            return new EventManagementDbContext(optionsBuilder.Options);
        }
    }
}