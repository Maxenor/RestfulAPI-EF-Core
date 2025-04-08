using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation specific to the Room entity.
    /// </summary>
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all rooms associated with a specific location asynchronously.
        /// </summary>
        /// <param name="locationId">The identifier of the location.</param>
        /// <returns>A read-only list of rooms for the specified location.</returns>
        public async Task<IReadOnlyList<Room>> GetRoomsByLocationIdAsync(int locationId)
        {
            return await _dbSet
                .Where(r => r.LocationId == locationId)
                .OrderBy(r => r.Name) // Optional: Order rooms by name
                .AsNoTracking()
                .ToListAsync();
        }
    }
}