namespace EventManagement.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ParticipantId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; } 

        // Navigation Properties
        public virtual Session Session { get; set; } = null!; 
        public virtual Participant Participant { get; set; } = null!;
    }
}