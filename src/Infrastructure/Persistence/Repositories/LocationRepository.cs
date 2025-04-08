using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
// using System.Collections.Generic; // Uncomment if adding specific methods
// using System.Threading.Tasks; // Uncomment if adding specific methods

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation specific to the Location entity.
    /// </summary>
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        // Implement ILocationRepository specific methods here if any are added later.
        // Example:
        // public async Task<IReadOnlyList<Location>> GetLocationsByCityAsync(string city)
        // {
        //     return await _dbSet.Where(l => l.City == city).AsNoTracking().ToListAsync();
        // }
    }
}