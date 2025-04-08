using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation specific to the Rating entity.
    /// </summary>
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all ratings submitted for a specific session asynchronously.
        /// </summary>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>A read-only list of ratings for the specified session.</returns>
        public async Task<IReadOnlyList<Rating>> GetRatingsBySessionIdAsync(int sessionId)
        {
            return await _dbSet
                .Where(r => r.SessionId == sessionId)
                .Include(r => r.Participant) // Optionally include participant details
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets all ratings submitted by a specific participant asynchronously.
        /// </summary>
        /// <param name="participantId">The identifier of the participant.</param>
        /// <returns>A read-only list of ratings submitted by the specified participant.</returns>
        public async Task<IReadOnlyList<Rating>> GetRatingsByParticipantIdAsync(int participantId)
        {
             return await _dbSet
                .Where(r => r.ParticipantId == participantId)
                .Include(r => r.Session) // Optionally include session details
                    .ThenInclude(s => s.Event) // Optionally include event details
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Calculates the average rating score for a specific session asynchronously.
        /// </summary>
        /// <param name="sessionId">The identifier of the session.</param>
        /// <returns>The average score, or null if there are no ratings for the session.</returns>
        public async Task<double?> GetAverageRatingForSessionAsync(int sessionId)
        {
            // Check if any ratings exist first to avoid division by zero or incorrect average on empty set
            bool hasRatings = await _dbSet.AnyAsync(r => r.SessionId == sessionId);
            if (!hasRatings)
            {
                return null; // Or return 0.0, depending on requirements
            }

            // Calculate average directly in the database
            return await _dbSet
                .Where(r => r.SessionId == sessionId)
                .AverageAsync(r => r.Score);
        }
    }
}