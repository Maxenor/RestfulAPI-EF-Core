using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int LocationId { get; set; } // Foreign Key

        // Navigation Properties
        public virtual Location Location { get; set; } = null!; // Required reference navigation property
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}