using AutoMapper;
using EventManagement.Application.DTOs.Location;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(
            ILocationRepository locationRepository, 
            IMapper mapper, 
            ILogger<LocationService> logger, 
            IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all locations");
            var locations = await _locationRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<LocationDto>>(locations);
        }

        public async Task<LocationDto?> GetLocationByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving location with ID {LocationId}", id);
            var location = await _locationRepository.GetByIdAsync(id);
            return location == null ? null : _mapper.Map<LocationDto>(location);
        }

        public async Task<LocationDto> CreateLocationAsync(CreateLocationDto createLocationDto, CancellationToken cancellationToken = default)
        {
            if (createLocationDto == null)
            {
                throw new ArgumentNullException(nameof(createLocationDto));
            }

            _logger.LogInformation("Creating a new location with name {LocationName}", createLocationDto.Name);
            
            var location = _mapper.Map<Location>(createLocationDto);
            await _locationRepository.AddAsync(location);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<LocationDto>(location);
        }

        public async Task<bool> UpdateLocationAsync(int id, UpdateLocationDto updateLocationDto, CancellationToken cancellationToken = default)
        {
            if (updateLocationDto == null)
            {
                throw new ArgumentNullException(nameof(updateLocationDto));
            }

            if (id != updateLocationDto.Id)
            {
                throw new InvalidOperationException("ID mismatch between provided ID and DTO ID");
            }

            _logger.LogInformation("Updating location with ID {LocationId}", id);
            
            var existingLocation = await _locationRepository.GetByIdAsync(id);
            if (existingLocation == null)
            {
                _logger.LogWarning("Location with ID {LocationId} not found", id);
                return false;
            }

            _mapper.Map(updateLocationDto, existingLocation);
            await _locationRepository.UpdateAsync(existingLocation);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteLocationAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting location with ID {LocationId}", id);
            
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
            {
                _logger.LogWarning("Location with ID {LocationId} not found", id);
                return false;
            }

            await _locationRepository.DeleteAsync(location);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }
    }
}