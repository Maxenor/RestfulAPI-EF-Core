using System;
using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    /// <summary>
    /// Represents a specific session within an Event.
    /// </summary>
    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } // Optional
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventId { get; set; } // Foreign Key
        public int RoomId { get; set; } // Foreign Key

        // Navigation Properties
        public virtual Event Event { get; set; } = null!; // Required reference
        public virtual Room Room { get; set; } = null!; // Required reference
        public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; } = new List<SessionSpeaker>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}