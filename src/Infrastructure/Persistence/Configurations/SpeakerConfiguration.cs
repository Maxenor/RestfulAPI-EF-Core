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

            // Seed Data
            builder.HasData(
                new Speaker { Id = 1, FirstName = "Alice", LastName = "Johnson", Bio = "Expert in cloud technologies and DevOps practices.", Email = "alice.j@example.com", Company = "Cloud Solutions Inc." },
                new Speaker { Id = 2, FirstName = "Bob", LastName = "Smith", Bio = "Specialist in modern web development frameworks.", Email = "bob.s@example.com", Company = "WebDev Experts" },
                new Speaker { Id = 3, FirstName = "Charlie", LastName = "Brown", Bio = "Data scientist with a focus on machine learning applications.", Email = "charlie.b@example.com", Company = "Data Insights Co." },
                new Speaker { Id = 4, FirstName = "Diana", LastName = "Prince", Bio = "Agile coach and project management professional.", Email = "diana.p@example.com", Company = "Agile Transformations Ltd." }
            );

        }
    }
}