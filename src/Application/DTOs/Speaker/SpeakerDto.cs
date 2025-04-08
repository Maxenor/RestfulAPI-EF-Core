namespace EventManagement.Application.DTOs.Speaker
{
    /// <summary>
    /// Data Transfer Object for Speaker information.
    /// </summary>
    public class SpeakerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Company { get; set; }
    }
}