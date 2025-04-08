using EventManagement.Application.DTOs.Rating;
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
    /// API endpoints for managing Ratings.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(IRatingService ratingService, ILogger<RatingsController> logger)
        {
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all ratings.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of ratings.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RatingDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatings(CancellationToken cancellationToken)
        {
            try
            {
                var ratings = await _ratingService.GetAllRatingsAsync(cancellationToken);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving ratings.");
            }
        }

        /// <summary>
        /// Gets a specific rating by ID.
        /// </summary>
        /// <param name="id">The ID of the rating to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested rating.</returns>
        [HttpGet("{id:int}", Name = "GetRatingById")]
        [ProducesResponseType(typeof(RatingDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<RatingDto>> GetRatingById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var rating = await _ratingService.GetRatingByIdAsync(id, cancellationToken);
                if (rating == null)
                {
                    _logger.LogInformation("Rating with ID {RatingId} not found.", id);
                    return NotFound($"Rating with ID {id} not found.");
                }
                return Ok(rating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving rating with ID {RatingId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the rating.");
            }
        }

        /// <summary>
        /// Gets all ratings for a specific session.
        /// </summary>
        /// <param name="sessionId">The ID of the session.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of ratings for the specified session.</returns>
        [HttpGet("session/{sessionId:int}")]
        [ProducesResponseType(typeof(IEnumerable<RatingDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatingsBySessionId(int sessionId, CancellationToken cancellationToken)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsBySessionIdAsync(sessionId, cancellationToken);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings for session ID {SessionId}.", sessionId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving ratings for the session.");
            }
        }

        /// <summary>
        /// Gets all ratings submitted by a specific participant.
        /// </summary>
        /// <param name="participantId">The ID of the participant.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of ratings submitted by the specified participant.</returns>
        [HttpGet("participant/{participantId:int}")]
        [ProducesResponseType(typeof(IEnumerable<RatingDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatingsByParticipantId(int participantId, CancellationToken cancellationToken)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsByParticipantIdAsync(participantId, cancellationToken);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings for participant ID {ParticipantId}.", participantId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving ratings for the participant.");
            }
        }

        /// <summary>
        /// Gets the average rating score for a specific session.
        /// </summary>
        /// <param name="sessionId">The ID of the session.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The average rating score, or null if there are no ratings.</returns>
        [HttpGet("session/{sessionId:int}/average")]
        [ProducesResponseType(typeof(double?), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<double?>> GetAverageRatingForSession(int sessionId, CancellationToken cancellationToken)
        {
            try
            {
                var averageRating = await _ratingService.GetAverageRatingForSessionAsync(sessionId, cancellationToken);
                return Ok(averageRating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving average rating for session ID {SessionId}.", sessionId);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving average rating for the session.");
            }
        }

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="createRatingDto">The rating data to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created rating.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RatingDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<RatingDto>> CreateRating([FromBody] CreateRatingDto createRatingDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdRating = await _ratingService.CreateRatingAsync(createRatingDto, cancellationToken);
                return CreatedAtRoute("GetRatingById", new { id = createdRating.Id }, createdRating);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
            {
                _logger.LogWarning(ex, "Failed to create rating: related entity not found.");
                return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to create rating.");
                return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating rating.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the rating.");
            }
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="id">The ID of the rating to update.</param>
        /// <param name="updateRatingDto">The updated rating data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] UpdateRatingDto updateRatingDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != updateRatingDto.Id)
                {
                    return BadRequest("ID mismatch between route parameter and request body.");
                }

                bool success = await _ratingService.UpdateRatingAsync(id, updateRatingDto, cancellationToken);
                if (!success)
                {
                    return NotFound($"Rating with ID {id} not found.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
            {
                _logger.LogWarning(ex, "Failed to update rating: related entity not found.");
                return NotFound(new ProblemDetails { Title = "Not Found", Detail = ex.Message, Status = (int)HttpStatusCode.NotFound });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to update rating {RatingId}.", id);
                return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating rating {RatingId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the rating.");
            }
        }

        /// <summary>
        /// Deletes a rating by ID.
        /// </summary>
        /// <param name="id">The ID of the rating to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteRating(int id, CancellationToken cancellationToken)
        {
            try
            {
                bool success = await _ratingService.DeleteRatingAsync(id, cancellationToken);
                if (!success)
                {
                    return NotFound($"Rating with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rating {RatingId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the rating.");
            }
        }
    }
}