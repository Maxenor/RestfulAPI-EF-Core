using System.ComponentModel.DataAnnotations;

namespace EventManagement.Application.DTOs.Category
{
    /// <summary>
    /// DTO for updating an existing Category.
    /// </summary>
    public class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; } // Required to identify the category to update

        [Required]
        [StringLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }
    }
}