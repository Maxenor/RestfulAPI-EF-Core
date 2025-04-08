using EventManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface specific to the Rating entity.
    /// Inherits common CRUD operations from IGenericRepository.
    /// </summary>
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        /// <summary>
        /// Gets all ratings submitted for a specific session asynchronously.
        /// </summary>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>A read-only list of ratings for the specified session.</returns>
        Task<IReadOnlyList<Rating>> GetRatingsBySessionIdAsync(int sessionId);

        /// <summary>
        /// Gets all ratings submitted by a specific participant asynchronously.
        /// </summary>
        /// <param name="participantId">The identifier of the participant.</param>
        /// <returns>A read-only list of ratings submitted by the specified participant.</returns>
        Task<IReadOnlyList<Rating>> GetRatingsByParticipantIdAsync(int participantId);

        /// <summary>
        /// Calculates the average rating score for a specific session asynchronously.
        /// </summary>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>The average score, or null if there are no ratings for the session.</returns>
        Task<double?> GetAverageRatingForSessionAsync(int sessionId);

        // Add other Rating-specific query methods here if needed.
    }
}