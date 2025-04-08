using EventManagement.Domain.Entities; // For EventStatus enum
using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Event
{
    /// <summary>
    /// DTO for updating an existing event. Includes validation rules.
    /// </summary>
    public class UpdateEventDto
    {
        [Required(ErrorMessage = "Event ID is required for updates.")]
        public int Id { get; set; } // Include Id for identifying the event to update

        [Required(ErrorMessage = "Event title is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        // Add custom validation attribute or service-level validation to ensure EndDate >= StartDate
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Event status is required.")]
        [EnumDataType(typeof(EventStatus), ErrorMessage = "Invalid event status value.")]
        public EventStatus Status { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Category ID.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Location ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Location ID.")]
        public int LocationId { get; set; }
    }
}