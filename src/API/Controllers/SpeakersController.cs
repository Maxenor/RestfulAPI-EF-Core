using EventManagement.Application.DTOs.Speaker;
using EventManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.API.Controllers
{
    /// <summary>
    /// API endpoints for managing Speakers.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SpeakersController : ControllerBase
    {
        private readonly ISpeakerService _speakerService;
        private readonly ILogger<SpeakersController> _logger;

        public SpeakersController(ISpeakerService speakerService, ILogger<SpeakersController> logger)
        {
            _speakerService = speakerService ?? throw new ArgumentNullException(nameof(speakerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all speakers.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of speakers.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SpeakerDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<SpeakerDto>>> GetSpeakers(CancellationToken cancellationToken)
        {
            try
            {
                var speakers = await _speakerService.GetAllSpeakersAsync(cancellationToken);
                return Ok(speakers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving speakers.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving speakers.");
            }
        }

        /// <summary>
        /// Gets a specific speaker by ID.
        /// </summary>
        /// <param name="id">The ID of the speaker to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested speaker.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(SpeakerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<SpeakerDto>> GetSpeakerById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var speaker = await _speakerService.GetSpeakerByIdAsync(id, cancellationToken);
                if (speaker == null)
                {
                    _logger.LogInformation("Speaker with ID {SpeakerId} not found.", id);
                    return NotFound($"Speaker with ID {id} not found.");
                }
                return Ok(speaker);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving speaker with ID {SpeakerId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the speaker.");
            }
        }

        /// <summary>
        /// Creates a new speaker.
        /// </summary>
        /// <param name="createSpeakerDto">The speaker data to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created speaker.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SpeakerDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<SpeakerDto>> CreateSpeaker([FromBody] CreateSpeakerDto createSpeakerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdSpeaker = await _speakerService.CreateSpeakerAsync(createSpeakerDto, cancellationToken);
                return CreatedAtAction(nameof(GetSpeakerById), new { id = createdSpeaker.Id }, createdSpeaker);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to create speaker.");
                return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating speaker.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the speaker.");
            }
        }

        /// <summary>
        /// Updates an existing speaker.
        /// </summary>
        /// <param name="id">The ID of the speaker to update.</param>
        /// <param name="updateSpeakerDto">The updated speaker data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateSpeaker(int id, [FromBody] UpdateSpeakerDto updateSpeakerDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool success = await _speakerService.UpdateSpeakerAsync(id, updateSpeakerDto, cancellationToken);
                if (!success)
                {
                    return NotFound($"Speaker with ID {id} not found.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to update speaker {SpeakerId}.", id);
                return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating speaker {SpeakerId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the speaker.");
            }
        }

        /// <summary>
        /// Deletes a speaker by ID.
        /// </summary>
        /// <param name="id">The ID of the speaker to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteSpeaker(int id, CancellationToken cancellationToken)
        {
            try
            {
                bool success = await _speakerService.DeleteSpeakerAsync(id, cancellationToken);
                if (!success)
                {
                    return NotFound($"Speaker with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting speaker {SpeakerId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the speaker.");
            }
        }
    }
}