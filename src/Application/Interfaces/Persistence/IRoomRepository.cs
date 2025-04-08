using EventManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface specific to the Room entity.
    /// Inherits common CRUD operations from IGenericRepository.
    /// </summary>
    public interface IRoomRepository : IGenericRepository<Room>
    {
        /// <summary>
        /// Gets all rooms associated with a specific location asynchronously.
        /// </summary>
        /// <param name="locationId">The identifier of the location.</param>
        /// <returns>A read-only list of rooms for the specified location.</returns>
        Task<IReadOnlyList<Room>> GetRoomsByLocationIdAsync(int locationId);

        // Add other Room-specific query methods here if needed.
    }
}