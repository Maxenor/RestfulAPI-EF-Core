using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Location
{
    /// <summary>
    /// DTO for creating a new Location.
    /// </summary>
    public class CreateLocationDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number.")]
        public int? Capacity { get; set; }
    }
}