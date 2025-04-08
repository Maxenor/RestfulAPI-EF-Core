using EventManagement.Domain.Entities;
// using System.Collections.Generic; // Uncomment if adding specific methods returning collections
// using System.Threading.Tasks; // Uncomment if adding specific async methods

namespace EventManagement.Application.Interfaces.Persistence
{

    public interface ILocationRepository : IGenericRepository<Location>
    {
        // Add Location-specific query methods here if needed in the future.
        // Example:
        // Task<IReadOnlyList<Location>> GetLocationsByCityAsync(string city);
        // Task<bool> IsLocationNameUniqueAsync(string name);
    }
}