using System.Collections.Generic;

namespace EventManagement.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Properties
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}