using System;

namespace EventManagement.Domain.Entities
{
    /// <summary>
    /// Represents the attendance status of a participant for an event.
    /// </summary>
    public enum AttendanceStatus
    {
        Registered,
        Attended,
        Cancelled,
        NoShow
    }

    /// <summary>
    /// Join entity linking Events and Participants (Many-to-Many).
    /// Includes registration details.
    /// </summary>
    public class EventParticipant
    {
        // Composite Primary Key (configured in DbContext using Fluent API)
        public int EventId { get; set; } // Part of Composite PK, FK to Event
        public int ParticipantId { get; set; } // Part of Composite PK, FK to Participant

        public DateTime RegistrationDate { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }

        // Navigation Properties
        public virtual Event Event { get; set; } = null!; // Required reference
        public virtual Participant Participant { get; set; } = null!; // Required reference
    }
}