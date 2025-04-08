using AutoMapper;
using EventManagement.Application.DTOs.Session;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;
using Microsoft.Extensions.Logging; // Added for logging
using System;
using System.Collections.Generic;
using System.Linq; // Added for LINQ operations
using System.Threading.Tasks;

namespace EventManagement.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IEventRepository _eventRepository; // To validate EventId
        private readonly IRoomRepository _roomRepository;   // To validate RoomId
        private readonly IMapper _mapper;
        private readonly ILogger<SessionService> _logger; // Added logger

        public SessionService(
            ISessionRepository sessionRepository,
            IEventRepository eventRepository,
            IRoomRepository roomRepository,
            IMapper mapper,
            ILogger<SessionService> logger) // Added logger injection
        {
            _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize logger
        }

        public async Task<IEnumerable<SessionDto>> GetAllSessionsAsync()
        {
            _logger.LogInformation("Retrieving all sessions.");
            // Using ListAllAsync - Note: This might not load related Room/Speakers unless the implementation does.
            // If related data is missing, a specific repository method like GetAllSessionsWithDetailsAsync might be needed.
            var sessions = await _sessionRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<SessionDto>>(sessions);
        }

        public async Task<SessionDto?> GetSessionByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving session with ID: {SessionId}", id);
            // Use the specific method designed to include details
            var session = await _sessionRepository.GetSessionWithDetailsAsync(id);

            if (session == null)
            {
                _logger.LogWarning("Session with ID: {SessionId} not found.", id);
                return null;
            }
            return _mapper.Map<SessionDto>(session);
        }

        public async Task<SessionDto> CreateSessionAsync(CreateSessionDto createSessionDto)
        {
            _logger.LogInformation("Attempting to create a new session titled: {SessionTitle}", createSessionDto.Title);

            // Basic Validation
            if (createSessionDto.EndTime <= createSessionDto.StartTime)
            {
                _logger.LogError("Validation failed: EndTime must be after StartTime for session creation.");
                throw new ArgumentException("End time must be after start time.");
            }

            // Check if related entities exist
            var eventExists = await _eventRepository.GetByIdAsync(createSessionDto.EventId) != null;
            if (!eventExists)
            {
                _logger.LogError("Validation failed: Event with ID {EventId} not found during session creation.", createSessionDto.EventId);
                throw new KeyNotFoundException($"Event with ID {createSessionDto.EventId} not found.");
            }

            var roomExists = await _roomRepository.GetByIdAsync(createSessionDto.RoomId) != null;
            if (!roomExists)
            {
                _logger.LogError("Validation failed: Room with ID {RoomId} not found during session creation.", createSessionDto.RoomId);
                throw new KeyNotFoundException($"Room with ID {createSessionDto.RoomId} not found.");
            }

            // Map DTO to Entity
            var session = _mapper.Map<Session>(createSessionDto);

            // Add to repository
            var addedSession = await _sessionRepository.AddAsync(session);
            // SaveChangesAsync is likely handled by a Unit of Work or higher layer

            _logger.LogInformation("Successfully created session with ID: {SessionId}", addedSession.Id);

            // Fetch the created session with includes using the specific method
            var createdSessionWithDetails = await _sessionRepository.GetSessionWithDetailsAsync(addedSession.Id);

            if (createdSessionWithDetails == null)
            {
                // This shouldn't happen if AddAsync succeeded, but handle defensively
                _logger.LogError("Failed to retrieve newly created session with ID: {SessionId}", addedSession.Id);
                throw new InvalidOperationException($"Failed to retrieve newly created session with ID {addedSession.Id}.");
            }

            return _mapper.Map<SessionDto>(createdSessionWithDetails); // Map the fully loaded entity
        }

        public async Task<bool> UpdateSessionAsync(int id, UpdateSessionDto updateSessionDto)
        {
            _logger.LogInformation("Attempting to update session with ID: {SessionId}", id);
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session == null)
            {
                _logger.LogWarning("Update failed: Session with ID: {SessionId} not found.", id);
                return false;
            }

            // Basic Validation
            if (updateSessionDto.EndTime <= updateSessionDto.StartTime)
            {
                _logger.LogError("Validation failed: EndTime must be after StartTime for session update (ID: {SessionId}).", id);
                throw new ArgumentException("End time must be after start time.");
            }

            // Check if the new RoomId exists
            var roomExists = await _roomRepository.GetByIdAsync(updateSessionDto.RoomId) != null;
            if (!roomExists)
            {
                 _logger.LogError("Validation failed: Room with ID {RoomId} not found during session update (ID: {SessionId}).", updateSessionDto.RoomId, id);
                throw new KeyNotFoundException($"Room with ID {updateSessionDto.RoomId} not found.");
            }

            // Map updated fields from DTO to existing entity
            _mapper.Map(updateSessionDto, session);
            // Note: EventId is intentionally not updated here based on UpdateSessionDto definition

            await _sessionRepository.UpdateAsync(session);
            // SaveChangesAsync is likely handled by a Unit of Work or higher layer

            _logger.LogInformation("Successfully updated session with ID: {SessionId}", id);
            return true;
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            _logger.LogInformation("Attempting to delete session with ID: {SessionId}", id);
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session == null)
            {
                _logger.LogWarning("Deletion failed: Session with ID: {SessionId} not found.", id);
                return false;
            }

            await _sessionRepository.DeleteAsync(session);
            // SaveChangesAsync is likely handled by a Unit of Work or higher layer

            _logger.LogInformation("Successfully deleted session with ID: {SessionId}", id);
            return true;
        }
    }
}