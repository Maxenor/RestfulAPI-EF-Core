using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Event entity using Fluent API.
    /// </summary>
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // Table name (optional, EF Core uses DbSet property name by default)
            // builder.ToTable("Events");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Property configurations
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200); // Example max length

            builder.Property(e => e.Description)
                .HasMaxLength(2000); // Example max length for description

            builder.Property(e => e.StartDate)
                .IsRequired();

            builder.Property(e => e.EndDate)
                .IsRequired();

            // Configure Enum to string storage (recommended for readability)
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50); // Ensure length accommodates enum string values

            // Foreign Keys (EF Core convention often handles this, but explicit is clearer)
            builder.Property(e => e.CategoryId)
                .IsRequired();

            builder.Property(e => e.LocationId)
                .IsRequired();

            // Relationships

            // One-to-Many: Category -> Events
            builder.HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting category if events exist

            // One-to-Many: Location -> Events
            builder.HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting location if events exist

            // One-to-Many: Event -> Sessions
            builder.HasMany(e => e.Sessions)
                .WithOne(s => s.Event)
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting an event cascades to its sessions

            // Many-to-Many: Event <-> Participant (through EventParticipant)
            // Configuration for the join entity EventParticipant will handle the details.
            // We still define the navigation property relationship here.
            builder.HasMany(e => e.EventParticipants)
                .WithOne(ep => ep.Event)
                .HasForeignKey(ep => ep.EventId);

            // Indexes (optional, but good for performance on filtered columns)
            builder.HasIndex(e => e.StartDate);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.CategoryId);
            builder.HasIndex(e => e.LocationId);
        }
    }
}