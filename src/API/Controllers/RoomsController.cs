using EventManagement.Application.DTOs.Room;
using EventManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // For ValidationException
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Optional: For logging

namespace EventManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomsController> _logger; // Optional logging

        public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
        {
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Handle logger null if injected
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of rooms.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoomDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms(CancellationToken cancellationToken)
        {
            var rooms = await _roomService.GetAllRoomsAsync(cancellationToken);
            return Ok(rooms);
        }

        /// <summary>
        /// Gets a specific room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested room.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoomDto>> GetRoomById(int id, CancellationToken cancellationToken)
        {
            var room = await _roomService.GetRoomByIdAsync(id, cancellationToken);
            if (room == null)
            {
                _logger.LogInformation("Room with ID {RoomId} not found.", id);
                return NotFound();
            }
            return Ok(room);
        }

        /// <summary>
        /// Creates a new room.
        /// </summary>
        /// <param name="createRoomDto">The room data to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created room.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomDto createRoomDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdRoom = await _roomService.CreateRoomAsync(createRoomDto, cancellationToken);
                // Return 201 Created with the location of the new resource and the resource itself
                return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.Id }, createdRoom);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed during room creation: {ValidationMessage}", ex.Message);
                // Return a more specific error message if needed
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex) // Catch unexpected errors
            {
                _logger.LogError(ex, "An error occurred while creating a room.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        /// <param name="id">The ID of the room to update.</param>
        /// <param name="updateRoomDto">The updated room data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto updateRoomDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _roomService.UpdateRoomAsync(id, updateRoomDto, cancellationToken);
                if (!success)
                {
                    _logger.LogInformation("Update failed: Room with ID {RoomId} not found.", id);
                    return NotFound();
                }
                return NoContent(); // Standard response for successful PUT
            }
            catch (ValidationException ex)
            {
                 _logger.LogWarning("Validation failed during room update for ID {RoomId}: {ValidationMessage}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex) // Catch unexpected errors
            {
                _logger.LogError(ex, "An error occurred while updating room with ID {RoomId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Deletes a room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoom(int id, CancellationToken cancellationToken)
        {
             try
            {
                var success = await _roomService.DeleteRoomAsync(id, cancellationToken);
                if (!success)
                {
                    _logger.LogInformation("Deletion failed: Room with ID {RoomId} not found.", id);
                    return NotFound();
                }
                return NoContent(); // Standard response for successful DELETE
            }
            catch (Exception ex) // Catch unexpected errors
            {
                _logger.LogError(ex, "An error occurred while deleting room with ID {RoomId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}