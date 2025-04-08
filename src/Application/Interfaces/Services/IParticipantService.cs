using EventManagement.Application.DTOs.Common;
using EventManagement.Application.DTOs.Event; // For EventListDto
using EventManagement.Application.DTOs.Participant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Defines the contract for participant management services.
    /// Handles business logic related to participants.
    /// </summary>
    public interface IParticipantService
    {
        /// <summary>
        /// Retrieves a paginated list of participants.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of participants per page.</param>
        /// <returns>A task representing the asynchronous operation, containing the paginated result of ParticipantDto.</returns>
        Task<PagedResult<ParticipantDto>> GetParticipantsAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves detailed information for a specific participant by their ID.
        /// </summary>
        /// <param name="id">The identifier of the participant.</param>
        /// <returns>A task representing the asynchronous operation, containing the ParticipantDto if found; otherwise, null.</returns>
        Task<ParticipantDto?> GetParticipantByIdAsync(int id);

        /// <summary>
        /// Creates a new participant based on the provided data.
        /// </summary>
        /// <param name="createParticipantDto">The DTO containing data for the new participant.</param>
        /// <returns>A task representing the asynchronous operation, containing the created ParticipantDto.</returns>
        /// <exception cref="ValidationException">Thrown when input data is invalid.</exception>
        /// <exception cref="DuplicateException">Thrown when a participant with the same email already exists.</exception>
        Task<ParticipantDto> CreateParticipantAsync(CreateParticipantDto createParticipantDto);

        /// <summary>
        /// Updates an existing participant.
        /// </summary>
        /// <param name="updateParticipantDto">The DTO containing updated data for the participant.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ValidationException">Thrown when input data is invalid.</exception>
        /// <exception cref="NotFoundException">Thrown when the participant is not found.</exception>
        /// <exception cref="DuplicateException">Thrown when updating the email conflicts with an existing participant.</exception>
        Task UpdateParticipantAsync(UpdateParticipantDto updateParticipantDto);

        /// <summary>
        /// Deletes a participant by their ID.
        /// Consider implications (e.g., anonymizing vs. hard delete, handling registrations).
        /// </summary>
        /// <param name="id">The identifier of the participant to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown when the participant is not found.</exception>
        Task DeleteParticipantAsync(int id);

        /// <summary>
        /// Retrieves the event registration history for a specific participant.
        /// </summary>
        /// <param name="participantId">The identifier of the participant.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of EventListDto.</returns>
        /// <exception cref="NotFoundException">Thrown when the participant is not found.</exception>
        Task<List<EventListDto>> GetParticipantEventHistoryAsync(int participantId);
    }
}