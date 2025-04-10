using EventManagement.Application.DTOs.Location;

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for managing Location entities.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Retrieves all locations.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of Location DTOs.</returns>
        Task<IEnumerable<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific location by its ID.
        /// </summary>
        /// <param name="id">The ID of the location.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Location DTO or null if not found.</returns>
        Task<LocationDto?> GetLocationByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new location.
        /// </summary>
        /// <param name="createLocationDto">The DTO containing data for the new location.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created Location DTO.</returns>
        Task<LocationDto> CreateLocationAsync(CreateLocationDto createLocationDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing location.
        /// </summary>
        /// <param name="id">The ID of the location to update.</param>
        /// <param name="updateLocationDto">The DTO containing the updated data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> UpdateLocationAsync(int id, UpdateLocationDto updateLocationDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a location by its ID.
        /// </summary>
        /// <param name="id">The ID of the location to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        Task<bool> DeleteLocationAsync(int id, CancellationToken cancellationToken = default);
    }
}