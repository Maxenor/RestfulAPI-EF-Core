using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Session
{
    /// <summary>
    /// DTO for creating a new Session.
    /// </summary>
    public class CreateSessionDto
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
        [Range(1, int.MaxValue, ErrorMessage = "EventId must be a positive integer.")]
        public int EventId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "RoomId must be a positive integer.")]
        public int RoomId { get; set; }

        // Speaker IDs might be added here later if needed for creation
        // public List<int> SpeakerIds { get; set; } = new List<int>();
    }
}