using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation specific to the Session entity.
    /// </summary>
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all sessions associated with a specific event, ordered by start time, asynchronously.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>A read-only list of sessions for the specified event, ordered by StartTime.</returns>
        public async Task<IReadOnlyList<Session>> GetSessionsByEventIdAsync(int eventId)
        {
            return await _dbSet
                .Where(s => s.EventId == eventId)
                .Include(s => s.Room) // Include Room details
                .Include(s => s.SessionSpeakers) // Include join table...
                    .ThenInclude(ss => ss.Speaker) // ...to get Speaker details
                .OrderBy(s => s.StartTime)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets a single session by its ID, including related details like Speakers, Room, etc.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The detailed session if found; otherwise, null.</returns>
        public async Task<Session?> GetSessionWithDetailsAsync(int sessionId)
        {
            return await _dbSet
                .Where(s => s.Id == sessionId)
                .Include(s => s.Room)
                .Include(s => s.Event) // Include parent Event details
                    .ThenInclude(e => e.Location) // Include Event's Location
                .Include(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker) // Include Speakers for the session
                .Include(s => s.Ratings) // Include Ratings for the session
                    .ThenInclude(r => r.Participant) // Optionally include Participant who rated
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}