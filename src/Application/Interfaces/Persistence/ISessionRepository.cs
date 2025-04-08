using EventManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface specific to the Session entity.
    /// Inherits common CRUD operations from IGenericRepository.
    /// </summary>
    public interface ISessionRepository : IGenericRepository<Session>
    {
        /// <summary>
        /// Gets all sessions associated with a specific event, ordered by start time, asynchronously.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>A read-only list of sessions for the specified event, ordered by StartTime.</returns>
        Task<IReadOnlyList<Session>> GetSessionsByEventIdAsync(int eventId);

        /// <summary>
        /// Gets a single session by its ID, including related details like Speakers, Room, etc.
        /// The specific related entities to include should be determined by the implementation.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The detailed session if found; otherwise, null.</returns>
        Task<Session?> GetSessionWithDetailsAsync(int sessionId);

        // Add other Session-specific query methods here if needed.
        // Example: Check for time conflicts within a room/event
        // Task<bool> HasTimeConflictAsync(int roomId, DateTime startTime, DateTime endTime, int? excludeSessionId = null);
    }
}