using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Speaker
{
    /// <summary>
    /// DTO for updating an existing Speaker.
    /// </summary>
    public class UpdateSpeakerDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Bio cannot be longer than 1000 characters.")]
        public string? Bio { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255, ErrorMessage = "Email cannot be longer than 255 characters.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Company name cannot be longer than 200 characters.")]
        public string? Company { get; set; }
    }
}