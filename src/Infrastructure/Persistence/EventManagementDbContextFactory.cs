using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System; // Required for Version
using System.IO; // Required for Path

namespace EventManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Factory for creating EventManagementDbContext instances during design time (e.g., for migrations).
    /// This allows EF Core tools to create migrations without needing a running application or database connection.
    /// </summary>
    public class EventManagementDbContextFactory : IDesignTimeDbContextFactory<EventManagementDbContext>
    {
        public EventManagementDbContext CreateDbContext(string[] args)
        {
            // Get the configuration from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("MariaDbConnection");
            
            // If connection string is not found, use a fallback with values from .env
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=localhost;Port=3307;Database=db;Uid=root;Pwd=rootpassword;";
            }

            // Specify the MariaDB server version used in docker-compose.yml (10.6)
            var serverVersion = new MariaDbServerVersion(new Version(10, 6, 0));

            var optionsBuilder = new DbContextOptionsBuilder<EventManagementDbContext>();
            optionsBuilder.UseMySql(connectionString, serverVersion,
                // Ensure migrations are generated in the correct assembly (Infrastructure)
                mySqlOptions => mySqlOptions.MigrationsAssembly(typeof(EventManagementDbContextFactory).Assembly.FullName)
            );

            return new EventManagementDbContext(optionsBuilder.Options);
        }
    }
}