using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the Category entity using Fluent API.
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            // Relationships

            // One-to-Many: Category -> Events (Inverse side configured in EventConfiguration)
            // builder.HasMany(c => c.Events)
            //     .WithOne(e => e.Category)
            //     .HasForeignKey(e => e.CategoryId); // Already defined in EventConfiguration

            // Indexes
            builder.HasIndex(c => c.Name)
                   .IsUnique(); // Ensure category names are unique
        }
    }
}