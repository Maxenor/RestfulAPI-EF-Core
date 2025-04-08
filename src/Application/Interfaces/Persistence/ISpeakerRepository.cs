using EventManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface specific to the Speaker entity.
    /// Inherits common CRUD operations from IGenericRepository.
    /// </summary>
    public interface ISpeakerRepository : IGenericRepository<Speaker>
    {
        /// <summary>
        /// Gets a speaker by their email address asynchronously. Assumes email is unique.
        /// </summary>
        /// <param name="email">The email address of the speaker.</param>
        /// <returns>The speaker if found; otherwise, null.</returns>
        Task<Speaker?> GetSpeakerByEmailAsync(string email);

        /// <summary>
        /// Gets all speakers associated with a specific session asynchronously.
        /// Requires joining with the SessionSpeaker entity.
        /// </summary>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>A read-only list of speakers for the specified session.</returns>
        Task<IReadOnlyList<Speaker>> GetSpeakersForSessionAsync(int sessionId);

        // Add other Speaker-specific query methods here if needed.
    }
}