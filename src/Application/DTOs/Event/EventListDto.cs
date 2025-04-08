using EventManagement.Application.DTOs.Category;
using EventManagement.Application.DTOs.Location;
using EventManagement.Domain.Entities; // For EventStatus enum
using System;

namespace EventManagement.Application.DTOs.Event
{
    /// <summary>
    /// DTO for displaying event information in lists.
    /// </summary>
    public class EventListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus Status { get; set; }
        public CategoryDto Category { get; set; } = null!; // Include nested Category DTO
        public LocationDto Location { get; set; } = null!; // Include nested Location DTO
    }
}