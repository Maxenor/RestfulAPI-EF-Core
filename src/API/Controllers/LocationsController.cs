using EventManagement.Application.DTOs.Location;
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
    /// API endpoints for managing Locations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationService locationService, ILogger<LocationsController> logger)
        {
            _locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all locations.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of locations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LocationDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations(CancellationToken cancellationToken)
        {
            try
            {
                var locations = await _locationService.GetAllLocationsAsync(cancellationToken);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving locations.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving locations.");
            }
        }

        /// <summary>
        /// Gets a specific location by ID.
        /// </summary>
        /// <param name="id">The ID of the location to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested location.</returns>
        [HttpGet("{id:int}", Name = "GetLocationById")]
        [ProducesResponseType(typeof(LocationDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<LocationDto>> GetLocationById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var location = await _locationService.GetLocationByIdAsync(id, cancellationToken);
                if (location == null)
                {
                    _logger.LogInformation("Location with ID {LocationId} not found.", id);
                    return NotFound($"Location with ID {id} not found.");
                }
                return Ok(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving location with ID {LocationId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the location.");
            }
        }

        /// <summary>
        /// Creates a new location.
        /// </summary>
        /// <param name="createLocationDto">The location data to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created location.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LocationDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<LocationDto>> CreateLocation([FromBody] CreateLocationDto createLocationDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdLocation = await _locationService.CreateLocationAsync(createLocationDto, cancellationToken);
                return CreatedAtRoute("GetLocationById", new { id = createdLocation.Id }, createdLocation);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to create location.");
                return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating location.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the location.");
            }
        }

        /// <summary>
        /// Updates an existing location.
        /// </summary>
        /// <param name="id">The ID of the location to update.</param>
        /// <param name="updateLocationDto">The updated location data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] UpdateLocationDto updateLocationDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != updateLocationDto.Id)
                {
                    return BadRequest("ID mismatch between route parameter and request body.");
                }

                bool success = await _locationService.UpdateLocationAsync(id, updateLocationDto, cancellationToken);
                if (!success)
                {
                    return NotFound($"Location with ID {id} not found.");
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to update location {LocationId}.", id);
                return BadRequest(new ProblemDetails { Title = "Bad Request", Detail = ex.Message, Status = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating location {LocationId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the location.");
            }
        }

        /// <summary>
        /// Deletes a location by ID.
        /// </summary>
        /// <param name="id">The ID of the location to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteLocation(int id, CancellationToken cancellationToken)
        {
            try
            {
                bool success = await _locationService.DeleteLocationAsync(id, cancellationToken);
                if (!success)
                {
                    return NotFound($"Location with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting location {LocationId}.", id);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the location.");
            }
        }
    }
}