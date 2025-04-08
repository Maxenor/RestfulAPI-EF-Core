using EventManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface specific to the Participant entity.
    /// Inherits common CRUD operations from IGenericRepository.
    /// </summary>
    public interface IParticipantRepository : IGenericRepository<Participant>
    {
        /// <summary>
        /// Gets a participant by their email address asynchronously. Assumes email is unique.
        /// </summary>
        /// <param name="email">The email address of the participant.</param>
        /// <returns>The participant if found; otherwise, null.</returns>
        Task<Participant?> GetParticipantByEmailAsync(string email);

        /// <summary>
        /// Gets all participants registered for a specific event asynchronously.
        /// Requires joining with the EventParticipant entity.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>A read-only list of participants registered for the event.</returns>
        Task<IReadOnlyList<Participant>> GetParticipantsForEventAsync(int eventId);

        /// <summary>
        /// Gets all events a specific participant is registered for asynchronously.
        /// Requires joining with the EventParticipant entity.
        /// </summary>
        /// <param name="participantId">The identifier of the participant.</param>
        /// <returns>A read-only list of events the participant is registered for.</returns>
        Task<IReadOnlyList<Event>> GetEventsForParticipantAsync(int participantId);

        // Add other Participant-specific query methods here if needed.
    }
}