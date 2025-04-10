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
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<SessionSpeaker> SessionSpeakers { get; set; }

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