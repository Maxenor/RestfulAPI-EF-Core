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

            // Seed Data
            builder.HasData(
                new Event
                {
                    Id = 1,
                    Title = "Tech Conference 2025",
                    Description = "Annual conference focusing on the latest trends in technology, AI, and software development.",
                    StartDate = new DateTime(2025, 10, 15, 9, 0, 0, DateTimeKind.Utc), // Use UtcNow or specific UTC dates
                    EndDate = new DateTime(2025, 10, 17, 17, 0, 0, DateTimeKind.Utc),
                    Status = EventStatus.Published,
                    CategoryId = 1, // Assuming Category ID 1 is 'Technology'
                    LocationId = 1  // Assuming Location ID 1 is 'Conference Center A'
                },
                new Event
                {
                    Id = 2,
                    Title = "Art Exhibition: Modern Masters",
                    Description = "A curated collection of modern art from renowned artists.",
                    StartDate = new DateTime(2025, 11, 5, 10, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 12, 20, 18, 0, 0, DateTimeKind.Utc),
                    Status = EventStatus.Published,
                    CategoryId = 2, // Assuming Category ID 2 is 'Arts & Culture'
                    LocationId = 2  // Assuming Location ID 2 is 'City Art Gallery'
                },
                new Event
                {
                    Id = 3,
                    Title = "Community Charity Run",
                    Description = "5k charity run to support local community projects. All ages welcome.",
                    StartDate = new DateTime(2025, 9, 20, 8, 30, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 9, 20, 11, 0, 0, DateTimeKind.Utc),
                    Status = EventStatus.Completed, // Assuming this event already happened
                    CategoryId = 3, // Assuming Category ID 3 is 'Community'
                    LocationId = 3  // Assuming Location ID 3 is 'Central Park'
                },
                new Event
                {
                    Id = 4,
                    Title = "Music Festival: Summer Sounds",
                    Description = "Weekend music festival featuring diverse genres and artists.",
                    StartDate = new DateTime(2025, 8, 1, 14, 0, 0, DateTimeKind.Utc), // Past event
                    EndDate = new DateTime(2025, 8, 3, 23, 0, 0, DateTimeKind.Utc),
                    Status = EventStatus.Completed,
                    CategoryId = 4, // Assuming Category ID 4 is 'Music'
                    LocationId = 1  // Back at Conference Center A (maybe outdoor area)
                },
                 new Event
                 {
                     Id = 5,
                     Title = "Advanced C# Workshop",
                     Description = "Deep dive into advanced C# features and .NET internals.",
                     StartDate = new DateTime(2025, 11, 25, 9, 0, 0, DateTimeKind.Utc),
                     EndDate = new DateTime(2025, 11, 26, 16, 30, 0, DateTimeKind.Utc),
                     Status = EventStatus.Published,
                     CategoryId = 1, // Technology
                     LocationId = 2  // City Art Gallery (maybe a specific room) - reusing location
                 }
            );
        }
    }
}