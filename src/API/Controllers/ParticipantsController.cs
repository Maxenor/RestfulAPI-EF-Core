using EventManagement.Application.DTOs.Common;
using EventManagement.Application.DTOs.Event; // For EventListDto
using EventManagement.Application.DTOs.Participant;
using EventManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
// using EventManagement.Application.Exceptions; // TODO: Use custom exceptions

namespace EventManagement.API.Controllers
{
    /// <summary>
    /// API endpoints for managing Participants.
    /// Inherits from BaseApiController for consistent routing.
    /// </summary>
    public class ParticipantsController : BaseApiController
    {
        private readonly IParticipantService _participantService;
        private readonly ILogger<ParticipantsController> _logger;

        public ParticipantsController(IParticipantService participantService, ILogger<ParticipantsController> logger)
        {
            _participantService = participantService ?? throw new ArgumentNullException(nameof(participantService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/v1/participants
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ParticipantDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PagedResult<ParticipantDto>>> GetParticipants(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
             try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page number and page size must be positive.");
                }
                var participants = await _participantService.GetParticipantsAsync(pageNumber, pageSize);
                return Ok(participants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving participants.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving participants.");
            }
        }

        // GET api/v1/participants/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ParticipantDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ParticipantDto>> GetParticipantById(int id)
        {
            try
            {
                var participant = await _participantService.GetParticipantByIdAsync(id);
                if (participant == null)
                {
                    _logger.LogInformation("Participant with ID {ParticipantId} not found.", id);
                    return NotFound($"Participant with ID {id} not found.");
                }
                return Ok(participant);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error retrieving participant with ID {ParticipantId}.", id);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the participant.");
            }
        }

        // POST api/v1/participants
        [HttpPost]
        [ProducesResponseType(typeof(ParticipantDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)] // For duplicate email
        public async Task<ActionResult<ParticipantDto>> CreateParticipant([FromBody] CreateParticipantDto createParticipantDto)
        {
            try
            {
                var createdParticipant = await _participantService.CreateParticipantAsync(createParticipantDto);
                return CreatedAtAction(nameof(GetParticipantById), new { id = createdParticipant.Id }, createdParticipant);
            }
            catch (InvalidOperationException ex) // Catch specific exceptions (e.g., duplicate email)
            {
                 _logger.LogWarning(ex, "Failed to create participant.");
                 // Check if it's a duplicate email error
                 if (ex.Message.Contains("already exists")) // Basic check, better with custom exceptions
                 {
                     return Conflict(new ProblemDetails { Title = "Conflict", Detail = ex.Message, Status = (int)HttpStatusCode.Conflict });
                 }
                 else
                 {
                     return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
                 }
            }
            // catch (DuplicateException ex) // TODO: Use custom DuplicateException
            // {
            //     _logger.LogWarning(ex, "Attempted to create participant with duplicate email {Email}.", createParticipantDto.Email);
            //     return Conflict(new ProblemDetails { Title = "Conflict", Detail = ex.Message, Status = (int)HttpStatusCode.Conflict });
            // }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error creating participant.");
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the participant.");
            }
        }

        // PUT api/v1/participants/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> UpdateParticipant(int id, [FromBody] UpdateParticipantDto updateParticipantDto)
        {
            if (id != updateParticipantDto.Id)
            {
                return BadRequest("ID mismatch between route parameter and request body.");
            }

            try
            {
                await _participantService.UpdateParticipantAsync(updateParticipantDto);
                return NoContent();
            }
            catch (InvalidOperationException ex) // Catch specific exceptions (NotFound, Duplicate)
            {
                 _logger.LogWarning(ex, "Failed to update participant {ParticipantId}.", id);
                 if (ex.Message.Contains("not found")) // TODO: Use NotFoundException
                 {
                     return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
                 }
                 else if (ex.Message.Contains("already exists")) // TODO: Use DuplicateException
                 {
                     return Conflict(new ProblemDetails { Title = "Conflict", Detail = ex.Message, Status = (int)HttpStatusCode.Conflict });
                 }
                 else
                 {
                      return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
                 }
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error updating participant {ParticipantId}.", id);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the participant.");
            }
        }

        // DELETE api/v1/participants/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)] // For business rule violations (e.g., cannot delete if registered)
        public async Task<IActionResult> DeleteParticipant(int id)
        {
             try
            {
                await _participantService.DeleteParticipantAsync(id);
                return NoContent();
            }
             catch (InvalidOperationException ex) // Catch specific exceptions (NotFound, CannotDelete)
            {
                 _logger.LogWarning(ex, "Failed to delete participant {ParticipantId}.", id);
                 if (ex.Message.Contains("not found")) // TODO: Use NotFoundException
                 {
                     return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
                 }
                 else // Assume business rule violation
                 {
                      return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
                 }
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error deleting participant {ParticipantId}.", id);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the participant.");
            }
        }

        // GET api/v1/participants/{id}/events
        [HttpGet("{id:int}/events")]
        [ProducesResponseType(typeof(List<EventListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<EventListDto>>> GetParticipantEventHistory(int id)
        {
            try
            {
                var events = await _participantService.GetParticipantEventHistoryAsync(id);
                // Service should throw if participant not found, caught below
                return Ok(events);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("not found")) // TODO: Use NotFoundException
            {
                _logger.LogInformation("Participant with ID {ParticipantId} not found when retrieving event history.", id);
                return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, "Error retrieving event history for participant {ParticipantId}.", id);
                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving event history.");
            }
        }
    }
}