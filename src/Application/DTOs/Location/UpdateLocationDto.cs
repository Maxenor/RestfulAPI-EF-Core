using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Location
{
    /// <summary>
    /// DTO for updating an existing Location.
    /// </summary>
    public class UpdateLocationDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Name { get; set; } // Nullable allows partial updates

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive number if provided.")]
        public int? Capacity { get; set; }
    }
}