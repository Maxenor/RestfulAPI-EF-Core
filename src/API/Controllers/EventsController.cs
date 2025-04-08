using EventManagement.Application.DTOs.Common;
using EventManagement.Application.DTOs.Event;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities; // For EventStatus enum
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Optional: For logging
using System;
using System.Net;
using System.Threading.Tasks;
// using EventManagement.Application.Exceptions; // TODO: Use custom exceptions

namespace EventManagement.API.Controllers
{
    /// <summary>
    /// Base controller providing common routing.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class BaseApiController : ControllerBase { }

    /// <summary>
    /// API endpoints for managing Events.
    /// </summary>
    public class EventsController : BaseApiController
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger; // Optional logger

        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/v1/events
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<EventListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PagedResult<EventListDto>>> GetEvents(
            [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate,
            [FromQuery] int? locationId, [FromQuery] int? categoryId, [FromQuery] EventStatus? status,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Basic validation for pagination parameters
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page number and page size must be positive.");
                }
                var events = await _eventService.GetEventsAsync(startDate, endDate, locationId, categoryId, status, pageNumber, pageSize);
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving events.");
                // Return a generic server error response
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving events.");
            }
        }

        // GET api/v1/events/{id}
        [HttpGet("{id:int}")] // Route constraint for integer ID
        [ProducesResponseType(typeof(EventDetailDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<EventDetailDto>> GetEventById(int id)
        {
            try
            {
                var eventDetail = await _eventService.GetEventByIdAsync(id);
                if (eventDetail == null)
                {
                    _logger.LogInformation("Event with ID {EventId} not found.", id);
                    return NotFound($"Event with ID {id} not found.");
                }
                return Ok(eventDetail);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error retrieving event with ID {EventId}.", id);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the event.");
            }
        }

        // POST api/v1/events
        [HttpPost]
        [ProducesResponseType(typeof(EventDetailDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)] // For validation errors or FK issues
        public async Task<ActionResult<EventDetailDto>> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            // ModelState validation is handled automatically by [ApiController]

            try
            {
                var createdEvent = await _eventService.CreateEventAsync(createEventDto);
                // Return 201 Created with Location header pointing to the new resource
                return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
            }
            catch (InvalidOperationException ex) // Catch specific exceptions from service layer
            {
                _logger.LogWarning(ex, "Failed to create event due to invalid operation (e.g., FK not found).");
                // Return BadRequest for known issues like non-existent FKs
                return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            }
            // catch (ValidationException ex) // TODO: Use custom ValidationException
            // {
            //     _logger.LogWarning(ex, "Failed to create event due to validation errors.");
            //     return BadRequest(new ProblemDetails { Title = "Validation Error", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            // }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error creating event.");
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the event.");
            }
        }

        // PUT api/v1/events/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDto updateEventDto)
        {
            if (id != updateEventDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }

            // ModelState validation handled by framework

            try
            {
                await _eventService.UpdateEventAsync(updateEventDto);
                return NoContent(); // Standard response for successful PUT
            }
            catch (InvalidOperationException ex) // Catch specific exceptions (NotFound, Validation)
            {
                 _logger.LogWarning(ex, "Failed to update event {EventId}.", id);
                 // Determine if it was NotFound or other validation based on exception message/type
                 if (ex.Message.Contains("not found")) // Basic check, better with custom exceptions
                 {
                     return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
                 }
                 else
                 {
                     return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
                 }
            }
            // catch (NotFoundException ex) // TODO: Use custom NotFoundException
            // {
            //     _logger.LogWarning(ex, "Event {EventId} not found for update.", id);
            //     return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
            // }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error updating event {EventId}.", id);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the event.");
            }
        }

        // DELETE api/v1/events/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteEvent(int id)
        {
             try
            {
                await _eventService.DeleteEventAsync(id);
                return NoContent(); // Standard response for successful DELETE
            }
             catch (InvalidOperationException ex) when (ex.Message.Contains("not found")) // TODO: Use NotFoundException
            {
                 _logger.LogWarning("Attempted to delete non-existent event {EventId}.", id);
                 return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error deleting event {EventId}.", id);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the event.");
            }
        }

        // --- Event Participant Management ---

        // POST api/v1/events/{eventId}/participants/{participantId}
        [HttpPost("{eventId:int}/participants/{participantId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)] // Or BadRequest
        public async Task<IActionResult> RegisterParticipant(int eventId, int participantId)
        {
            try
            {
                await _eventService.RegisterParticipantAsync(eventId, participantId);
                return NoContent();
            }
            catch (InvalidOperationException ex) // Catch specific exceptions
            {
                _logger.LogWarning(ex, "Failed registration for Event {EventId}, Participant {ParticipantId}.", eventId, participantId);
                if (ex.Message.Contains("not found")) // TODO: Use NotFoundException
                {
                    return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
                }
                else // Assume conflict or other business rule violation
                {
                     return Conflict(new ProblemDetails { Title = "Conflict", Detail = ex.Message, Status = (int)HttpStatusCode.Conflict });
                }
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error registering participant {ParticipantId} for event {EventId}.", participantId, eventId);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred during registration.");
            }
        }

        // DELETE api/v1/events/{eventId}/participants/{participantId}
        [HttpDelete("{eventId:int}/participants/{participantId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UnregisterParticipant(int eventId, int participantId)
        {
             try
            {
                await _eventService.UnregisterParticipantAsync(eventId, participantId);
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("not found") || ex.Message.Contains("not registered")) // TODO: Use NotFoundException
            {
                _logger.LogWarning(ex, "Failed unregistration for Event {EventId}, Participant {ParticipantId}.", eventId, participantId);
                return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error unregistering participant {ParticipantId} from event {EventId}.", participantId, eventId);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred during unregistration.");
            }
        }
    }
}