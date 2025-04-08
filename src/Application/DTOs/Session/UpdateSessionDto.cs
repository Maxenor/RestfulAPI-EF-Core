using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Session
{
    /// <summary>
    /// DTO for updating an existing Session.
    /// </summary>
    public class UpdateSessionDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        // Add validation: EndTime must be after StartTime (can be done in service or via custom validation attribute)
        public DateTime EndTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "RoomId must be a positive integer.")]
        public int RoomId { get; set; }

        // Note: EventId is typically not updated. If needed, add it here.
        // Speaker updates would likely be handled via separate endpoints or logic.
    }
}