namespace EventManagement.Application.DTOs.Room
{
    /// <summary>
    /// Data Transfer Object for Room information.
    /// </summary>
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int LocationId { get; set; } // Keep LocationId for context if needed
        // Optionally include LocationDto if needed: public LocationDto Location { get; set; }
    }
}