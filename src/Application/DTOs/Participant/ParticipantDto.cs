namespace EventManagement.Application.DTOs.Participant
{
    /// <summary>
    /// Data Transfer Object for Participant information.
    /// </summary>
    public class ParticipantDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
        // Avoid exposing sensitive data or internal details like registration dates here.
        // If needed, create a specific EventParticipantDto.
    }
}