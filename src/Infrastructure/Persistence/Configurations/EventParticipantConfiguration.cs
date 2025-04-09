using System;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the EventParticipant join entity using Fluent API.
    /// </summary>
    public class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
    {
        public void Configure(EntityTypeBuilder<EventParticipant> builder)
        {
            // Define Composite Primary Key
            builder.HasKey(ep => new { ep.EventId, ep.ParticipantId });

            // Configure Enum to string storage
            builder.Property(ep => ep.AttendanceStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50); // Ensure length accommodates enum string values

            builder.Property(ep => ep.RegistrationDate)
                .IsRequired();

            // Configure Relationships (Many-to-Many)

            // Relationship to Event
            builder.HasOne(ep => ep.Event)
                .WithMany(e => e.EventParticipants) // Navigation property in Event entity
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade); // If an event is deleted, remove registrations

            // Relationship to Participant
            builder.HasOne(ep => ep.Participant)
                .WithMany(p => p.EventParticipants) // Navigation property in Participant entity
                .HasForeignKey(ep => ep.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade); // If a participant is deleted, remove registrations

            // Indexes (optional, but often useful on foreign keys)
            builder.HasIndex(ep => ep.ParticipantId);
            // Composite key index is created automatically

            // Seed Data
            builder.HasData(
                new EventParticipant { EventId = 1, ParticipantId = 1, AttendanceStatus = AttendanceStatus.Registered, RegistrationDate = new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 1, ParticipantId = 2, AttendanceStatus = AttendanceStatus.Registered, RegistrationDate = new DateTime(2025, 1, 16, 11, 30, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 2, ParticipantId = 1, AttendanceStatus = AttendanceStatus.Attended, RegistrationDate = new DateTime(2025, 2, 1, 9, 0, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 2, ParticipantId = 3, AttendanceStatus = AttendanceStatus.Registered, RegistrationDate = new DateTime(2025, 2, 5, 14, 0, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 3, ParticipantId = 3, AttendanceStatus = AttendanceStatus.Cancelled, RegistrationDate = new DateTime(2025, 3, 10, 16, 0, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 3, ParticipantId = 4, AttendanceStatus = AttendanceStatus.Registered, RegistrationDate = new DateTime(2025, 3, 11, 10, 0, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 4, ParticipantId = 4, AttendanceStatus = AttendanceStatus.Attended, RegistrationDate = new DateTime(2025, 4, 1, 12, 0, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 4, ParticipantId = 5, AttendanceStatus = AttendanceStatus.Registered, RegistrationDate = new DateTime(2025, 4, 2, 13, 0, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 5, ParticipantId = 1, AttendanceStatus = AttendanceStatus.Registered, RegistrationDate = new DateTime(2025, 5, 5, 9, 30, 0, DateTimeKind.Utc) },
                new EventParticipant { EventId = 5, ParticipantId = 5, AttendanceStatus = AttendanceStatus.Attended, RegistrationDate = new DateTime(2025, 5, 6, 11, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}