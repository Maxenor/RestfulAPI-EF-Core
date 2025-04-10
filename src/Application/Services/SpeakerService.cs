using AutoMapper;
using EventManagement.Application.DTOs.Speaker;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Services
{
    public class SpeakerService : ISpeakerService
    {
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SpeakerService> _logger;

        public SpeakerService(ISpeakerRepository speakerRepository, IMapper mapper, ILogger<SpeakerService> logger)
        {
            _speakerRepository = speakerRepository ?? throw new ArgumentNullException(nameof(speakerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<SpeakerDto>> GetAllSpeakersAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all speakers.");
            var speakers = await _speakerRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<SpeakerDto>>(speakers);
        }

        public async Task<SpeakerDto?> GetSpeakerByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving speaker with ID: {SpeakerId}", id);
            var speaker = await _speakerRepository.GetByIdAsync(id);
            if (speaker == null)
            {
                _logger.LogWarning("Speaker with ID: {SpeakerId} not found.", id);
                return null;
            }
            return _mapper.Map<SpeakerDto>(speaker);
        }

        public async Task<SpeakerDto> CreateSpeakerAsync(CreateSpeakerDto createSpeakerDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new speaker.");
            var speaker = _mapper.Map<Speaker>(createSpeakerDto);
            var createdSpeaker = await _speakerRepository.AddAsync(speaker);
            _logger.LogInformation("Speaker created successfully with ID: {SpeakerId}", createdSpeaker.Id);
            return _mapper.Map<SpeakerDto>(createdSpeaker);
        }

        public async Task<bool> UpdateSpeakerAsync(int id, UpdateSpeakerDto updateSpeakerDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to update speaker with ID: {SpeakerId}", id);
            var speakerToUpdate = await _speakerRepository.GetByIdAsync(id);

            if (speakerToUpdate == null)
            {
                _logger.LogWarning("Update failed. Speaker with ID: {SpeakerId} not found.", id);
                return false;
            }

            _mapper.Map(updateSpeakerDto, speakerToUpdate);
            await _speakerRepository.UpdateAsync(speakerToUpdate);
            _logger.LogInformation("Speaker with ID: {SpeakerId} updated successfully.", id);
            return true;
        }

        public async Task<bool> DeleteSpeakerAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to delete speaker with ID: {SpeakerId}", id);
            // GetByIdAsync doesn't accept a cancellationToken
            var speakerToDelete = await _speakerRepository.GetByIdAsync(id);

            if (speakerToDelete == null)
            {
                _logger.LogWarning("Delete failed. Speaker with ID: {SpeakerId} not found.", id);
                return false;
            }

            // DeleteAsync doesn't accept a cancellationToken
            await _speakerRepository.DeleteAsync(speakerToDelete);
            _logger.LogInformation("Speaker with ID: {SpeakerId} deleted successfully.", id);
            return true;
        }
    }
}