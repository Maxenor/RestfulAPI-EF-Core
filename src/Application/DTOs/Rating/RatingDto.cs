namespace EventManagement.Application.DTOs.Rating
{
    /// <summary>
    /// DTO for Rating information.
    /// </summary>
    public class RatingDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ParticipantId { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        
        // Optional: Navigation properties if needed for UI display
        public string SessionTitle { get; set; } = string.Empty;
        public string ParticipantName { get; set; } = string.Empty;
    }
}