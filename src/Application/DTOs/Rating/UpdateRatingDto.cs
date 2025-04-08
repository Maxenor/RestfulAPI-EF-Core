using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Rating
{
    /// <summary>
    /// DTO for updating an existing Rating.
    /// </summary>
    public class UpdateRatingDto
    {
        [Required(ErrorMessage = "Rating ID is required for updates.")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Session ID is required.")]
        public int SessionId { get; set; }
        
        [Required(ErrorMessage = "Participant ID is required.")]
        public int ParticipantId { get; set; }
        
        [Required(ErrorMessage = "Score is required.")]
        [Range(1, 5, ErrorMessage = "Score must be between 1 and 5.")]
        public int Score { get; set; }
        
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string? Comment { get; set; }
    }
}