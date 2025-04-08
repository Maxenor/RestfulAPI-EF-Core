using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Event?> GetEventWithDetailsAsync(int id)
        {
            // Eagerly load related entities needed for the detailed view
            return await _dbSet
                .Include(e => e.Category)
                .Include(e => e.Location)
                    .ThenInclude(l => l.Rooms) // Example: Include rooms within the location
                .Include(e => e.Sessions)
                    .ThenInclude(s => s.Room) // Include room for each session
                .Include(e => e.Sessions)
                    .ThenInclude(s => s.SessionSpeakers)
                        .ThenInclude(ss => ss.Speaker) // Include speakers for each session
                .Include(e => e.EventParticipants)
                    .ThenInclude(ep => ep.Participant) // Include participants registered for the event
                .AsNoTracking() // Use AsNoTracking for read-only queries
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IReadOnlyList<Event>> FindEventsAsync(
            DateTime? startDate, DateTime? endDate, int? locationId, int? categoryId, EventStatus? status,
            int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Include(e => e.Category) // Include necessary related data for list view
                .Include(e => e.Location)
                .AsNoTracking(); // Read-only query

            // Apply filters
            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                // Assuming filter means events ending on or before this date
                query = query.Where(e => e.EndDate <= endDate.Value);
                // Or maybe events that *start* before this date? Clarify requirement if needed.
                // query = query.Where(e => e.StartDate <= endDate.Value);
            }
            if (locationId.HasValue)
            {
                query = query.Where(e => e.LocationId == locationId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == categoryId.Value);
            }
            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            // Apply pagination
            // Ensure pageNumber is at least 1
            pageNumber = Math.Max(1, pageNumber);
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Order results (e.g., by start date)
            query = query.OrderBy(e => e.StartDate);

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalEventCountAsync(
            DateTime? startDate, DateTime? endDate, int? locationId, int? categoryId, EventStatus? status)
        {
             var query = _dbSet.AsQueryable(); // No need for AsNoTracking or Includes for Count

            // Apply the same filters as FindEventsAsync
            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                 query = query.Where(e => e.EndDate <= endDate.Value);
                // query = query.Where(e => e.StartDate <= endDate.Value); // Match filter logic used above
            }
            if (locationId.HasValue)
            {
                query = query.Where(e => e.LocationId == locationId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == categoryId.Value);
            }
            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            return await query.CountAsync();
        }

        // Override other methods from GenericRepository if specific Event logic is needed
        // e.g., custom logic before deleting an event.
    }
}