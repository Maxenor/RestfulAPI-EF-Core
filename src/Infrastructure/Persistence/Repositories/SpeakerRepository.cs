using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation specific to the Speaker entity.
    /// </summary>
    public class SpeakerRepository : GenericRepository<Speaker>, ISpeakerRepository
    {
        public SpeakerRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets a speaker by their email address asynchronously. Assumes email is unique.
        /// </summary>
        /// <param name="email">The email address of the speaker.</param>
        /// <returns>The speaker if found; otherwise, null.</returns>
        public async Task<Speaker?> GetSpeakerByEmailAsync(string email)
        {
            // Consider case-insensitivity based on requirements
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Gets all speakers associated with a specific session asynchronously.
        /// </summary>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>A read-only list of speakers for the specified session.</returns>
        public async Task<IReadOnlyList<Speaker>> GetSpeakersForSessionAsync(int sessionId)
        {
            // Query the join table (SessionSpeakers) and include the Speaker details
            return await _dbContext.SessionSpeakers
                .Where(ss => ss.SessionId == sessionId)
                .Include(ss => ss.Speaker) // Include the related Speaker entity
                .Select(ss => ss.Speaker) // Select only the Speaker
                .OrderBy(s => s.LastName).ThenBy(s => s.FirstName) // Order speakers
                .AsNoTracking()
                .ToListAsync();
        }
    }
}