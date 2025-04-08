using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Participant
{
    /// <summary>
    /// DTO for updating an existing participant. Includes validation rules.
    /// </summary>
    public class UpdateParticipantDto
    {
        [Required(ErrorMessage = "Participant ID is required for updates.")]
        public int Id { get; set; } // Include Id for identifying the participant to update

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(254, ErrorMessage = "Email address cannot exceed 254 characters.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters.")]
        public string? Company { get; set; }

        [StringLength(100, ErrorMessage = "Job title cannot exceed 100 characters.")]
        public string? JobTitle { get; set; }
    }
}