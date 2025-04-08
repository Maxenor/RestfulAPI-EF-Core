using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Room
{
    /// <summary>
    /// DTO for updating an existing Room.
    /// </summary>
    public class UpdateRoomDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Room name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1.")]
        public int Capacity { get; set; }

        // Assuming LocationId might not be updatable, but including for completeness.
        // Service logic should handle whether this can be changed.
        [Required(ErrorMessage = "LocationId is required.")]
        public int LocationId { get; set; }
    }
}