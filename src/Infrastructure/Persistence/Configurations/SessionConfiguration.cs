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
        }
    }
}