namespace EventManagement.Application.DTOs.Location
{
    /// <summary>
    /// Data Transfer Object for Location information.
    /// </summary>
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int? Capacity { get; set; } // Nullable to match entity
    }
}