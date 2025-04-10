using EventManagement.Application.DTOs.Common;
using EventManagement.Application.DTOs.Event;
using EventManagement.Domain.Entities; // For EventStatus enum

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Defines the contract for event management services.
    /// Handles business logic related to events.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Retrieves a paginated list of events based on filter criteria.
        /// </summary>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="locationId">Optional location filter.</param>
        /// <param name="categoryId">Optional category filter.</param>
        /// <param name="status">Optional status filter.</param>
        /// <param name="pageNumber">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of events per page.</param>
        /// <returns>A task representing the asynchronous operation, containing the paginated result of EventListDto.</returns>
        Task<PagedResult<EventListDto>> GetEventsAsync(
            DateTime? startDate, DateTime? endDate, int? locationId, int? categoryId, EventStatus? status,
            int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves detailed information for a specific event by its ID.
        /// </summary>
        /// <param name="id">The identifier of the event.</param>
        /// <returns>A task representing the asynchronous operation, containing the EventDetailDto if found; otherwise, null.</returns>
        Task<EventDetailDto?> GetEventByIdAsync(int id);

        /// <summary>
        /// Creates a new event based on the provided data.
        /// </summary>
        /// <param name="createEventDto">The DTO containing data for the new event.</param>
        /// <returns>A task representing the asynchronous operation, containing the created EventDetailDto.</returns>
        /// <exception cref="ValidationException">Thrown when input data is invalid.</exception>
        /// <exception cref="NotFoundException">Thrown when related entities (Category, Location) are not found.</exception>
        Task<EventDetailDto> CreateEventAsync(CreateEventDto createEventDto);

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="updateEventDto">The DTO containing updated data for the event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ValidationException">Thrown when input data is invalid.</exception>
        /// <exception cref="NotFoundException">Thrown when the event or related entities are not found.</exception>
        Task UpdateEventAsync(UpdateEventDto updateEventDto);

        /// <summary>
        /// Deletes an event by its ID.
        /// </summary>
        /// <param name="id">The identifier of the event to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown when the event is not found.</exception>
        Task DeleteEventAsync(int id);

        /// <summary>
        /// Registers a participant for a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <param name="participantId">The identifier of the participant.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown when the event or participant is not found.</exception>
        /// <exception cref="ApplicationException">Thrown for business rule violations (e.g., already registered, event full).</exception>
        Task RegisterParticipantAsync(int eventId, int participantId);

        /// <summary>
        /// Unregisters a participant from a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the event.</param>
        /// <param name="participantId">The identifier of the participant.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Thrown when the event, participant, or registration is not found.</exception>
        Task UnregisterParticipantAsync(int eventId, int participantId);
    }
}