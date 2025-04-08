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
        }
    }
}