using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Location entity using Fluent API.
    /// </summary>
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(l => l.Address)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(l => l.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Capacity);
                

            // Relationships

            // One-to-Many: Location -> Rooms
            builder.HasMany(l => l.Rooms)
                .WithOne(r => r.Location)
                .HasForeignKey(r => r.LocationId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a location cascades to its rooms

            // One-to-Many: Location -> Events (Inverse side configured in EventConfiguration)
            // builder.HasMany(l => l.Events)
            //     .WithOne(e => e.Location)
            //     .HasForeignKey(e => e.LocationId); // Already defined in EventConfiguration

            // Indexes
            builder.HasIndex(l => new { l.Name, l.City, l.Country }); // Example composite index

            // Seed Data
            builder.HasData(
                new Location { Id = 1, Name = "Conference Center A", Address = "123 Main St", City = "New York", Country = "USA", Capacity = 500 },
                new Location { Id = 2, Name = "Tech Hub B", Address = "456 Innovation Dr", City = "San Francisco", Country = "USA", Capacity = 300 },
                new Location { Id = 3, Name = "Event Hall C", Address = "789 Community Ave", City = "London", Country = "UK", Capacity = 1000 }
            );
        }
    }
}