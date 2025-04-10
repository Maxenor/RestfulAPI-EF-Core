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

    public class EventParticipant
    {
        public int EventId { get; set; }
        public int ParticipantId { get; set; }

        public DateTime RegistrationDate { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }

        // Navigation Properties
        public virtual Event Event { get; set; } = null!;
        public virtual Participant Participant { get; set; } = null!;
    }
}