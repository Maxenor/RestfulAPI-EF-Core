using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Speaker entity using Fluent API.
    /// </summary>
    public class SpeakerConfiguration : IEntityTypeConfiguration<Speaker>
    {
        public void Configure(EntityTypeBuilder<Speaker> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Bio)
                .HasMaxLength(2000);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(254);

            builder.Property(s => s.Company)
                .HasMaxLength(200);

            // Relationships

            // Many-to-Many: Session <-> Speaker (through SessionSpeaker)
            builder.HasMany(s => s.SessionSpeakers)
                .WithOne(ss => ss.Speaker)
                .HasForeignKey(ss => ss.SpeakerId);

            // Indexes
            // Consider if Email should be unique for Speakers. If so:
            // builder.HasIndex(s => s.Email).IsUnique();
            // If a speaker can also be a participant, uniqueness might be handled differently or not enforced here.
        }
    }
}