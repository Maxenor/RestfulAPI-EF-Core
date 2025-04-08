using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    public class Speaker
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Bio { get; set; } // Optional
        public string Email { get; set; } = string.Empty; // Consider uniqueness constraint
        public string? Company { get; set; } // Optional

        // Navigation Properties
        public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; } = new List<SessionSpeaker>();
    }
}