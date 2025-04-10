using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    public class Participant
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Company { get; set; }
        public string? JobTitle { get; set; }

        // Navigation Properties
        public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}