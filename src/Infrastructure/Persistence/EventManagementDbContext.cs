using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection; // Required for ApplyConfigurationsFromAssembly

namespace EventManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Represents the database context for the Event Management application.
    /// </summary>
    public class EventManagementDbContext : DbContext
    {
        // DbSet properties for each entity represent tables in the database
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<Speaker> Speakers { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<EventParticipant> EventParticipants { get; set; } = null!;
        public DbSet<SessionSpeaker> SessionSpeakers { get; set; } = null!;

        /// <param name="options">The options for this context.</param>
        public EventManagementDbContext(DbContextOptions<EventManagementDbContext> options)
            : base(options)
        {
        }

        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all entity configurations defined in the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}