using AutoMapper;
using EventManagement.Application.DTOs.Rating;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Services
{
    /// <summary>
    /// Service for managing Rating entities.
    /// </summary>
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RatingService> _logger;

        public RatingService(
            IRatingRepository ratingRepository,
            ISessionRepository sessionRepository,
            IParticipantRepository participantRepository,
            IMapper mapper,
            ILogger<RatingService> logger)
        {
            _ratingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(ratingRepository));
            _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            _participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RatingDto>> GetAllRatingsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all ratings");
            var ratings = await _ratingRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<RatingDto?> GetRatingByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving rating with ID: {RatingId}", id);
            var rating = await _ratingRepository.GetByIdAsync(id);
            if (rating == null)
            {
                _logger.LogWarning("Rating with ID: {RatingId} not found", id);
                return null;
            }
            return _mapper.Map<RatingDto>(rating);
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving ratings for session with ID: {SessionId}", sessionId);
            var ratings = await _ratingRepository.GetRatingsBySessionIdAsync(sessionId);
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsByParticipantIdAsync(int participantId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving ratings by participant with ID: {ParticipantId}", participantId);
            var ratings = await _ratingRepository.GetRatingsByParticipantIdAsync(participantId);
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<double?> GetAverageRatingForSessionAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Calculating average rating for session with ID: {SessionId}", sessionId);
            return await _ratingRepository.GetAverageRatingForSessionAsync(sessionId);
        }

        public async Task<RatingDto> CreateRatingAsync(CreateRatingDto createRatingDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new rating for session {SessionId} by participant {ParticipantId}", 
                createRatingDto.SessionId, createRatingDto.ParticipantId);

            // Validate that session exists
            var session = await _sessionRepository.GetByIdAsync(createRatingDto.SessionId);
            if (session == null)
            {
                _logger.LogWarning("Cannot create rating: Session with ID {SessionId} not found", createRatingDto.SessionId);
                throw new InvalidOperationException($"Session with ID {createRatingDto.SessionId} not found");
            }

            // Validate that participant exists
            var participant = await _participantRepository.GetByIdAsync(createRatingDto.ParticipantId);
            if (participant == null)
            {
                _logger.LogWarning("Cannot create rating: Participant with ID {ParticipantId} not found", createRatingDto.ParticipantId);
                throw new InvalidOperationException($"Participant with ID {createRatingDto.ParticipantId} not found");
            }

            // Map DTO to entity
            var rating = _mapper.Map<Rating>(createRatingDto);
            
            // Add the rating
            var createdRating = await _ratingRepository.AddAsync(rating);
            _logger.LogInformation("Rating created successfully with ID: {RatingId}", createdRating.Id);
            
            // Return mapped DTO
            return _mapper.Map<RatingDto>(createdRating);
        }

        public async Task<bool> UpdateRatingAsync(int id, UpdateRatingDto updateRatingDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating rating with ID: {RatingId}", id);
            
            if (id != updateRatingDto.Id)
            {
                _logger.LogWarning("Update failed: ID mismatch between route {RouteId} and body {BodyId}", id, updateRatingDto.Id);
                return false;
            }

            // Check if rating exists
            var ratingToUpdate = await _ratingRepository.GetByIdAsync(id);
            if (ratingToUpdate == null)
            {
                _logger.LogWarning("Update failed: Rating with ID: {RatingId} not found", id);
                return false;
            }

            // Validate that session exists
            var session = await _sessionRepository.GetByIdAsync(updateRatingDto.SessionId);
            if (session == null)
            {
                _logger.LogWarning("Cannot update rating: Session with ID {SessionId} not found", updateRatingDto.SessionId);
                throw new InvalidOperationException($"Session with ID {updateRatingDto.SessionId} not found");
            }

            // Validate that participant exists
            var participant = await _participantRepository.GetByIdAsync(updateRatingDto.ParticipantId);
            if (participant == null)
            {
                _logger.LogWarning("Cannot update rating: Participant with ID {ParticipantId} not found", updateRatingDto.ParticipantId);
                throw new InvalidOperationException($"Participant with ID {updateRatingDto.ParticipantId} not found");
            }

            // Map DTO to entity
            _mapper.Map(updateRatingDto, ratingToUpdate);
            
            // Update the rating
            await _ratingRepository.UpdateAsync(ratingToUpdate);
            _logger.LogInformation("Rating with ID: {RatingId} updated successfully", id);
            
            return true;
        }

        public async Task<bool> DeleteRatingAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting rating with ID: {RatingId}", id);
            
            // Check if rating exists
            var ratingToDelete = await _ratingRepository.GetByIdAsync(id);
            if (ratingToDelete == null)
            {
                _logger.LogWarning("Delete failed: Rating with ID: {RatingId} not found", id);
                return false;
            }

            // Delete the rating
            await _ratingRepository.DeleteAsync(ratingToDelete);
            _logger.LogInformation("Rating with ID: {RatingId} deleted successfully", id);
            
            return true;
        }
    }
}