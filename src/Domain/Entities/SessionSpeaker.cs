namespace EventManagement.Domain.Entities
{
    public class SessionSpeaker
    {
        public int SessionId { get; set; } 
        public int SpeakerId { get; set; } 

        public string? Role { get; set; }

        // Navigation Properties
        public virtual Session Session { get; set; } = null!; 
        public virtual Speaker Speaker { get; set; } = null!; 
    }
}