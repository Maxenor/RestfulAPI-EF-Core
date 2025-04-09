using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the SessionSpeaker join entity using Fluent API.
    /// </summary>
    public class SessionSpeakerConfiguration : IEntityTypeConfiguration<SessionSpeaker>
    {
        public void Configure(EntityTypeBuilder<SessionSpeaker> builder)
        {
            // Define Composite Primary Key
            builder.HasKey(ss => new { ss.SessionId, ss.SpeakerId });

            builder.Property(ss => ss.Role)
                .HasMaxLength(100); // Optional role description

            // Configure Relationships (Many-to-Many)

            // Relationship to Session
            builder.HasOne(ss => ss.Session)
                .WithMany(s => s.SessionSpeakers) // Navigation property in Session entity
                .HasForeignKey(ss => ss.SessionId)
                .OnDelete(DeleteBehavior.Cascade); // If a session is deleted, remove speaker assignments

            // Relationship to Speaker
            builder.HasOne(ss => ss.Speaker)
                .WithMany(s => s.SessionSpeakers) // Navigation property in Speaker entity
                .HasForeignKey(ss => ss.SpeakerId)
                .OnDelete(DeleteBehavior.Cascade); // If a speaker is deleted, remove session assignments

            // Indexes (optional)
            builder.HasIndex(ss => ss.SpeakerId);
            // Composite key index is created automatically

            // Seed Data for SessionSpeaker
            builder.HasData(
                new SessionSpeaker { SessionId = 1, SpeakerId = 1, Role = "Lead Speaker" },
                new SessionSpeaker { SessionId = 2, SpeakerId = 2, Role = "Panelist" },
                new SessionSpeaker { SessionId = 3, SpeakerId = 3, Role = "Keynote" },
                new SessionSpeaker { SessionId = 4, SpeakerId = 4, Role = "Workshop Facilitator" },
                new SessionSpeaker { SessionId = 5, SpeakerId = 1, Role = "Co-Speaker" },
                new SessionSpeaker { SessionId = 5, SpeakerId = 2, Role = "Co-Speaker" },
                new SessionSpeaker { SessionId = 6, SpeakerId = 3, Role = "Moderator" },
                new SessionSpeaker { SessionId = 6, SpeakerId = 4, Role = "Panelist" },
                new SessionSpeaker { SessionId = 7, SpeakerId = 1, Role = "Lead Speaker" },
                new SessionSpeaker { SessionId = 7, SpeakerId = 3, Role = "Guest Speaker" }
            );
        }
    }
}