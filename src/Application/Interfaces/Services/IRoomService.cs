using EventManagement.Application.DTOs.Room;

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for managing Room entities.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Retrieves all rooms.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of Room DTOs.</returns>
        Task<IEnumerable<RoomDto>> GetAllRoomsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Room DTO or null if not found.</returns>
        Task<RoomDto?> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new room.
        /// </summary>
        /// <param name="createRoomDto">The DTO containing room creation data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created Room DTO.</returns>
        Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        /// <param name="id">The ID of the room to update.</param>
        /// <param name="updateRoomDto">The DTO containing room update data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if update was successful, false otherwise.</returns>
        Task<bool> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deletion was successful, false otherwise.</returns>
        Task<bool> DeleteRoomAsync(int id, CancellationToken cancellationToken = default);
    }
}