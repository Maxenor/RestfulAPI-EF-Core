using EventManagement.Application.DTOs.Category;
using EventManagement.Application.DTOs.Location;
using EventManagement.Application.DTOs.Participant;
using EventManagement.Application.DTOs.Session;
using EventManagement.Domain.Entities; // For EventStatus enum
using System;
using System.Collections.Generic;

namespace EventManagement.Application.DTOs.Event
{
    /// <summary>
    /// DTO for displaying detailed event information.
    /// </summary>
    public class EventDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus Status { get; set; }
        public CategoryDto Category { get; set; } = null!;
        public LocationDto Location { get; set; } = null!;
        public List<SessionDto> Sessions { get; set; } = new List<SessionDto>(); // List of Session DTOs
        public List<ParticipantDto> Participants { get; set; } = new List<ParticipantDto>(); // List of Participant DTOs
    }
}