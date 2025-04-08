using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Rating entity using Fluent API.
    /// </summary>
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(r => r.Id);

            // Foreign Keys
            builder.Property(r => r.SessionId)
                .IsRequired();

            builder.Property(r => r.ParticipantId)
                .IsRequired();

            builder.Property(r => r.Score)
                .IsRequired();
                // Consider adding a check constraint for the score range (e.g., 1-5) if the DB supports it
                // .HasCheckConstraint("CK_Rating_Score", "[Score] >= 1 AND [Score] <= 5"); // Example for SQL Server

            builder.Property(r => r.Comment)
                .HasMaxLength(1000);

            // Relationships

            // One-to-Many: Session -> Ratings (Inverse side configured in SessionConfiguration)
            // builder.HasOne(r => r.Session)
            //     .WithMany(s => s.Ratings)
            //     .HasForeignKey(r => r.SessionId); // Already defined in SessionConfiguration

            // One-to-Many: Participant -> Ratings (Inverse side configured in ParticipantConfiguration)
            // builder.HasOne(r => r.Participant)
            //     .WithMany(p => p.Ratings)
            //     .HasForeignKey(r => r.ParticipantId); // Already defined in ParticipantConfiguration

            // Indexes
            builder.HasIndex(r => new { r.SessionId, r.ParticipantId })
                   .IsUnique(); // Ensure a participant can rate a session only once
        }
    }
}