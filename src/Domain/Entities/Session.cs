using System;
using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventId { get; set; }
        public int RoomId { get; set; }

        // Navigation Properties
        public virtual Event Event { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
        public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; } = new List<SessionSpeaker>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}