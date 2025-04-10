using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int? Capacity { get; set; }

        // Navigation Properties
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}