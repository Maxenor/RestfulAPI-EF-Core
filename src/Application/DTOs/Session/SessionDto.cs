using EventManagement.Application.DTOs.Room; // Assuming RoomDto exists or will be created
using EventManagement.Application.DTOs.Speaker; // Assuming SpeakerDto exists or will be created
using System;
using System.Collections.Generic;

namespace EventManagement.Application.DTOs.Session
{
    /// <summary>
    /// Data Transfer Object for Session information.
    /// </summary>
    public class SessionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventId { get; set; } // Keep EventId for context if needed
        public RoomDto Room { get; set; } = null!; // Nested Room DTO
        public List<SpeakerDto> Speakers { get; set; } = new List<SpeakerDto>(); // List of Speaker DTOs
        // Add average rating if needed: public double? AverageRating { get; set; }
    }
}