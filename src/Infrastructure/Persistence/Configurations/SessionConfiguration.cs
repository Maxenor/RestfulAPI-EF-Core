using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Session entity using Fluent API.
    /// </summary>
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Description)
                .HasMaxLength(2000);

            builder.Property(s => s.StartTime)
                .IsRequired();

            builder.Property(s => s.EndTime)
                .IsRequired();

            // Foreign Keys
            builder.Property(s => s.EventId)
                .IsRequired();

            builder.Property(s => s.RoomId)
                .IsRequired();

            // Relationships

            // One-to-Many: Event -> Sessions (Inverse side configured in EventConfiguration)
            // builder.HasOne(s => s.Event)
            //     .WithMany(e => e.Sessions)
            //     .HasForeignKey(s => s.EventId); // Already defined in EventConfiguration

            // One-to-Many: Room -> Sessions
            builder.HasOne(s => s.Room)
                .WithMany(r => r.Sessions)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting room if sessions are assigned

            // Many-to-Many: Session <-> Speaker (through SessionSpeaker)
            builder.HasMany(s => s.SessionSpeakers)
                .WithOne(ss => ss.Session)
                .HasForeignKey(ss => ss.SessionId);

            // One-to-Many: Session -> Ratings
            builder.HasMany(s => s.Ratings)
                .WithOne(r => r.Session)
                .HasForeignKey(r => r.SessionId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a session cascades to its ratings

            // Indexes
            builder.HasIndex(s => s.EventId);
            builder.HasIndex(s => s.RoomId);
            builder.HasIndex(s => s.StartTime);

            // Seed Data
            builder.HasData(
                new Session { Id = 1, EventId = 1, RoomId = 1, Title = "Introduction to C# 12", StartTime = new DateTime(2025, 10, 20, 9, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 20, 10, 30, 0, DateTimeKind.Utc), Description = "Overview of new features in C# 12." },
                new Session { Id = 2, EventId = 1, RoomId = 2, Title = "Advanced Async Patterns", StartTime = new DateTime(2025, 10, 20, 11, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 20, 12, 30, 0, DateTimeKind.Utc), Description = "Deep dive into asynchronous programming in .NET." },
                new Session { Id = 3, EventId = 2, RoomId = 3, Title = "Building Microservices with ASP.NET Core", StartTime = new DateTime(2025, 11, 15, 10, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 15, 11, 30, 0, DateTimeKind.Utc), Description = "Learn how to design and build microservices." },
                new Session { Id = 4, EventId = 3, RoomId = 4, Title = "Entity Framework Core Best Practices", StartTime = new DateTime(2025, 11, 15, 13, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 11, 15, 14, 30, 0, DateTimeKind.Utc), Description = "Tips and tricks for efficient data access with EF Core." },
                new Session { Id = 5, EventId = 4, RoomId = 5, Title = "Securing Web APIs", StartTime = new DateTime(2025, 12, 5, 9, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 5, 11, 0, 0, DateTimeKind.Utc), Description = "Authentication and authorization strategies for APIs." },
                new Session { Id = 6, EventId = 5, RoomId = 1, Title = "Introduction to Blazor", StartTime = new DateTime(2025, 12, 5, 11, 30, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 5, 13, 0, 0, DateTimeKind.Utc), Description = "Building interactive web UIs with C# and Blazor." },
                new Session { Id = 7, EventId = 1, RoomId = 3, Title = "Minimal APIs in .NET 8", StartTime = new DateTime(2025, 10, 20, 14, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 10, 20, 15, 30, 0, DateTimeKind.Utc), Description = "Creating lightweight APIs with minimal code." }
            );

        }
    }
}