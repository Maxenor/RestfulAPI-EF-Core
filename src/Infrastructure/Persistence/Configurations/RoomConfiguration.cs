using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Room entity using Fluent API.
    /// </summary>
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Capacity)
                .IsRequired();

            // Foreign Key
            builder.Property(r => r.LocationId)
                .IsRequired();

            // Relationships

            // One-to-Many: Location -> Rooms (Inverse side configured in LocationConfiguration)
            // builder.HasOne(r => r.Location)
            //     .WithMany(l => l.Rooms)
            //     .HasForeignKey(r => r.LocationId); // Already defined in LocationConfiguration

            // One-to-Many: Room -> Sessions (Inverse side configured in SessionConfiguration)
            // builder.HasMany(r => r.Sessions)
            //     .WithOne(s => s.Room)
            //     .HasForeignKey(s => s.RoomId); // Already defined in SessionConfiguration

            // Indexes
            builder.HasIndex(r => new { r.LocationId, r.Name })
                   .IsUnique(); // Ensure room names are unique within a location

            // Seed Data
            builder.HasData(
                new Room { Id = 1, Name = "Grand Ballroom", Capacity = 500, LocationId = 1 },
                new Room { Id = 2, Name = "Meeting Room 101", Capacity = 50, LocationId = 1 },
                new Room { Id = 3, Name = "Meeting Room 102", Capacity = 50, LocationId = 1 },
                new Room { Id = 4, Name = "Innovation Hall", Capacity = 300, LocationId = 2 },
                new Room { Id = 5, Name = "Workshop Alpha", Capacity = 75, LocationId = 2 },
                new Room { Id = 6, Name = "Lecture Hall C1", Capacity = 200, LocationId = 3 },
                new Room { Id = 7, Name = "Seminar Room C2", Capacity = 40, LocationId = 3 }
            );
        }
    }
}