using EventManagement.Application.DTOs.Session;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for managing Session entities.
    /// Defines the contract for session-related operations.
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Retrieves all sessions asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Session DTOs.</returns>
        Task<IEnumerable<SessionDto>> GetAllSessionsAsync();

        /// <summary>
        /// Retrieves a specific session by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the session to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the Session DTO if found, otherwise null.</returns>
        Task<SessionDto?> GetSessionByIdAsync(int id);

        /// <summary>
        /// Creates a new session asynchronously.
        /// </summary>
        /// <param name="createSessionDto">The DTO containing the data for the new session.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created Session DTO.</returns>
        Task<SessionDto> CreateSessionAsync(CreateSessionDto createSessionDto);

        /// <summary>
        /// Updates an existing session asynchronously.
        /// </summary>
        /// <param name="id">The ID of the session to update.</param>
        /// <param name="updateSessionDto">The DTO containing the updated data for the session.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful, otherwise false.</returns>
        Task<bool> UpdateSessionAsync(int id, UpdateSessionDto updateSessionDto);

        /// <summary>
        /// Deletes a session asynchronously.
        /// </summary>
        /// <param name="id">The ID of the session to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the deletion was successful, otherwise false.</returns>
        Task<bool> DeleteSessionAsync(int id);
    }
}