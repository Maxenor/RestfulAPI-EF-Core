using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Participant entity using Fluent API.
    /// </summary>
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(254); // Standard max length for email

            builder.Property(p => p.Company)
                .HasMaxLength(200);

            builder.Property(p => p.JobTitle)
                .HasMaxLength(100);

            // Relationships

            // Many-to-Many: Event <-> Participant (through EventParticipant)
            builder.HasMany(p => p.EventParticipants)
                .WithOne(ep => ep.Participant)
                .HasForeignKey(ep => ep.ParticipantId);

            // One-to-Many: Participant -> Ratings
            builder.HasMany(p => p.Ratings)
                .WithOne(r => r.Participant)
                .HasForeignKey(r => r.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade); // If a participant is deleted, their ratings are also deleted.

            // Indexes
            builder.HasIndex(p => p.Email)
                   .IsUnique(); // Ensure email addresses are unique


            // Seed Data
            builder.HasData(
                new Participant { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", Company = "TechCorp", JobTitle = "Software Engineer" },
                new Participant { Id = 2, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", Company = "Innovate Ltd.", JobTitle = "Project Manager" },
                new Participant { Id = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie.brown@sample.net", Company = "Data Solutions", JobTitle = "Data Analyst" },
                new Participant { Id = 4, FirstName = "Diana", LastName = "Prince", Email = "diana.prince@mail.org", Company = "Global Enterprises", JobTitle = "Marketing Specialist" },
                new Participant { Id = 5, FirstName = "Ethan", LastName = "Hunt", Email = "ethan.hunt@secure.com", Company = "Security Inc.", JobTitle = "Security Consultant" }
            );
        }
    }
}