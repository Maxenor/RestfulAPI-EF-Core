using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Room
{
    /// <summary>
    /// DTO for creating a new Room.
    /// </summary>
    public class CreateRoomDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Room name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "LocationId is required.")]
        public int LocationId { get; set; }
    }
}