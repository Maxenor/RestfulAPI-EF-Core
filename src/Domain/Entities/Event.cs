using System;
using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    public enum EventStatus
    {
        Draft,
        Published,
        Cancelled,
        Completed
    }

    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } // Optional
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus Status { get; set; }
        public int CategoryId { get; set; } // Foreign Key
        public int LocationId { get; set; } // Foreign Key

        // Navigation Properties
        public virtual Category Category { get; set; } = null!; // Required reference
        public virtual Location Location { get; set; } = null!; // Required reference
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
    }
}