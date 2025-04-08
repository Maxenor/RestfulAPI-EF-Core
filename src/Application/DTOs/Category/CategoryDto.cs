namespace EventManagement.Application.DTOs.Category
{
    /// <summary>
    /// Data Transfer Object for Category information.
    /// </summary>
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } // Nullable to match entity
    }
}