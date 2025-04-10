using EventManagement.Application.DTOs.Rating;

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for managing Rating entities.
    /// </summary>
    public interface IRatingService
    {
        /// <summary>
        /// Retrieves all ratings.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of Rating DTOs.</returns>
        Task<IEnumerable<RatingDto>> GetAllRatingsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific rating by its ID.
        /// </summary>
        /// <param name="id">The ID of the rating to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Rating DTO if found; otherwise, null.</returns>
        Task<RatingDto?> GetRatingByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all ratings for a specific session.
        /// </summary>
        /// <param name="sessionId">The ID of the session.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of Rating DTOs for the specified session.</returns>
        Task<IEnumerable<RatingDto>> GetRatingsBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all ratings submitted by a specific participant.
        /// </summary>
        /// <param name="participantId">The ID of the participant.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of Rating DTOs submitted by the specified participant.</returns>
        Task<IEnumerable<RatingDto>> GetRatingsByParticipantIdAsync(int participantId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the average rating score for a specific session.
        /// </summary>
        /// <param name="sessionId">The ID of the session.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The average rating score, or null if there are no ratings.</returns>
        Task<double?> GetAverageRatingForSessionAsync(int sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="createRatingDto">The DTO containing rating creation data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created Rating DTO.</returns>
        Task<RatingDto> CreateRatingAsync(CreateRatingDto createRatingDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="id">The ID of the rating to update.</param>
        /// <param name="updateRatingDto">The DTO containing rating update data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> UpdateRatingAsync(int id, UpdateRatingDto updateRatingDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a rating by its ID.
        /// </summary>
        /// <param name="id">The ID of the rating to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        Task<bool> DeleteRatingAsync(int id, CancellationToken cancellationToken = default);
    }
}