using EventManagement.Application.DTOs.Session;
using EventManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] // Updated route with versioning
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly ILogger<SessionsController> _logger;

        public SessionsController(ISessionService sessionService, ILogger<SessionsController> logger)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/v1/sessions
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessions()
        {
            try
            {
                var sessions = await _sessionService.GetAllSessionsAsync();
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all sessions.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        // GET: api/v1/sessions/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SessionDto>> GetSession(int id)
        {
            try
            {
                var session = await _sessionService.GetSessionByIdAsync(id);
                if (session == null)
                {
                    _logger.LogWarning("Session with ID: {SessionId} not found.", id);
                    return NotFound($"Session with ID {id} not found.");
                }
                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving session with ID: {SessionId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        // POST: api/v1/sessions
        [HttpPost]
        [ProducesResponseType(typeof(SessionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // For related entities not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SessionDto>> CreateSession([FromBody] CreateSessionDto createSessionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdSession = await _sessionService.CreateSessionAsync(createSessionDto);
                // Return 201 Created with the location header and the created resource
                return CreatedAtAction(nameof(GetSession), new { id = createdSession.Id }, createdSession);
            }
            catch (ArgumentException ex) // Specific validation errors (e.g., EndTime <= StartTime)
            {
                 _logger.LogWarning(ex, "Validation error during session creation: {ErrorMessage}", ex.Message);
                 return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex) // Related entity not found (Event, Room)
            {
                _logger.LogWarning(ex, "Related entity not found during session creation: {ErrorMessage}", ex.Message);
                return NotFound(ex.Message); // Return 404 if Event or Room doesn't exist
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a session.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        // PUT: api/v1/sessions/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] UpdateSessionDto updateSessionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _sessionService.UpdateSessionAsync(id, updateSessionDto);
                if (!success)
                {
                    _logger.LogWarning("Update failed: Session with ID: {SessionId} not found.", id);
                    return NotFound($"Session with ID {id} not found.");
                }
                return NoContent(); // Standard response for successful PUT
            }
            catch (ArgumentException ex) // Specific validation errors
            {
                 _logger.LogWarning(ex, "Validation error during session update (ID: {SessionId}): {ErrorMessage}", id, ex.Message);
                 return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex) // Related entity not found (Room)
            {
                _logger.LogWarning(ex, "Related entity not found during session update (ID: {SessionId}): {ErrorMessage}", id, ex.Message);
                return NotFound(ex.Message); // Return 404 if Room doesn't exist
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating session with ID: {SessionId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        // DELETE: api/v1/sessions/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSession(int id)
        {
            try
            {
                var success = await _sessionService.DeleteSessionAsync(id);
                if (!success)
                {
                    _logger.LogWarning("Deletion failed: Session with ID: {SessionId} not found.", id);
                    return NotFound($"Session with ID {id} not found.");
                }
                return NoContent(); // Standard response for successful DELETE
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting session with ID: {SessionId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }
    }
}