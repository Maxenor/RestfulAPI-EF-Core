namespace EventManagement.Domain.Entities
{
    /// <summary>
    /// Represents a participant's rating for a session.
    /// </summary>
    public class Rating
    {
        public int Id { get; set; }
        public int SessionId { get; set; } // Foreign Key
        public int ParticipantId { get; set; } // Foreign Key
        public int Score { get; set; } // e.g., 1 to 5
        public string? Comment { get; set; } // Optional

        // Navigation Properties
        public virtual Session Session { get; set; } = null!; // Required reference
        public virtual Participant Participant { get; set; } = null!; // Required reference
    }
}