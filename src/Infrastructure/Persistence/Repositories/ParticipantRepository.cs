using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation specific to the Participant entity.
    /// </summary>
    public class ParticipantRepository : GenericRepository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets a participant by their email address asynchronously. Assumes email is unique.
        /// </summary>
        /// <param name="email">The email address of the participant.</param>
        /// <returns>The participant if found; otherwise, null.</returns>
        public async Task<Participant?> GetParticipantByEmailAsync(string email)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Gets all participants registered for a specific event asynchronously.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <returns>A read-only list of participants registered for the event.</returns>
        public async Task<IReadOnlyList<Participant>> GetParticipantsForEventAsync(int eventId)
        {
            // Query the join table (EventParticipants) and include the Participant details
            return await _dbContext.EventParticipants
                .Where(ep => ep.EventId == eventId)
                .Include(ep => ep.Participant) // Include the related Participant entity
                .Select(ep => ep.Participant) // Select only the Participant
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName) // Order participants
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets all events a specific participant is registered for asynchronously.
        /// </summary>
        /// <param name="participantId">The identifier of the participant.</param>
        /// <returns>A read-only list of events the participant is registered for.</returns>
        public async Task<IReadOnlyList<Event>> GetEventsForParticipantAsync(int participantId)
        {
            // Query the join table (EventParticipants) and include the Event details
            return await _dbContext.EventParticipants
                .Where(ep => ep.ParticipantId == participantId)
                .Include(ep => ep.Event) // Include the related Event entity
                    .ThenInclude(e => e.Category) // Include event category
                .Include(ep => ep.Event)
                    .ThenInclude(e => e.Location) // Include event location
                .Select(ep => ep.Event) // Select only the Event
                .OrderBy(e => e.StartDate) // Order events by start date
                .AsNoTracking()
                .ToListAsync();
        }
    }
}