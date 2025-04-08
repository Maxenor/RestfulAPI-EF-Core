using EventManagement.Application.DTOs.Speaker;

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for managing Speaker entities.
    /// </summary>
    public interface ISpeakerService
    {
        /// <summary>
        /// Retrieves all speakers.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of Speaker DTOs.</returns>
        Task<IEnumerable<SpeakerDto>> GetAllSpeakersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific speaker by its ID.
        /// </summary>
        /// <param name="id">The ID of the speaker.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Speaker DTO if found; otherwise, null.</returns>
        Task<SpeakerDto?> GetSpeakerByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new speaker.
        /// </summary>
        /// <param name="createSpeakerDto">The DTO containing speaker creation data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created Speaker DTO.</returns>
        Task<SpeakerDto> CreateSpeakerAsync(CreateSpeakerDto createSpeakerDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing speaker.
        /// </summary>
        /// <param name="id">The ID of the speaker to update.</param>
        /// <param name="updateSpeakerDto">The DTO containing speaker update data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateSpeakerAsync(int id, UpdateSpeakerDto updateSpeakerDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a speaker by its ID.
        /// </summary>
        /// <param name="id">The ID of the speaker to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteSpeakerAsync(int id, CancellationToken cancellationToken = default);
    }
}