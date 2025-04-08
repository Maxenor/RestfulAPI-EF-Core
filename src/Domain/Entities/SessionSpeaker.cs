namespace EventManagement.Domain.Entities
{
    public class SessionSpeaker
    {
        public int SessionId { get; set; } // Part of Composite PK, FK to Session
        public int SpeakerId { get; set; } // Part of Composite PK, FK to Speaker

        public string? Role { get; set; }

        // Navigation Properties
        public virtual Session Session { get; set; } = null!; // Required reference
        public virtual Speaker Speaker { get; set; } = null!; // Required reference
    }
}