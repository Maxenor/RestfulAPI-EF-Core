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
        }
    }
}