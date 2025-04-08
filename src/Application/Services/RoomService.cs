using AutoMapper;
using EventManagement.Application.DTOs.Room;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // For ValidationException

namespace EventManagement.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILocationRepository _locationRepository; // To validate LocationId
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, ILocationRepository locationRepository, IMapper mapper)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync(CancellationToken cancellationToken = default) // Keep CancellationToken for service layer consistency if needed elsewhere, but repo doesn't use it
        {
            // Note: CancellationToken is not passed to the repository based on IGenericRepository
            var rooms = await _roomRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<RoomDto?> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // Note: CancellationToken is not passed to the repository based on IGenericRepository
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                // Consider throwing NotFoundException or returning null based on convention
                return null;
            }
            return _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto, CancellationToken cancellationToken = default)
        {
            // Validate LocationId exists
            // Note: CancellationToken is not passed to the repository based on IGenericRepository
            var locationExists = await _locationRepository.GetByIdAsync(createRoomDto.LocationId);
            if (locationExists == null)
            {
                throw new ValidationException($"Location with ID {createRoomDto.LocationId} does not exist.");
            }

            var room = _mapper.Map<Room>(createRoomDto);
            // Note: CancellationToken is not passed to the repository based on IGenericRepository
            var createdRoom = await _roomRepository.AddAsync(room);
            // Assuming SaveChanges is handled by Unit of Work or DbContext elsewhere

            return _mapper.Map<RoomDto>(createdRoom);
        }

        public async Task<bool> UpdateRoomAsync(int id, UpdateRoomDto updateRoomDto, CancellationToken cancellationToken = default)
        {
            // Note: CancellationToken is not passed to the repository based on IGenericRepository
            var roomToUpdate = await _roomRepository.GetByIdAsync(id);
            if (roomToUpdate == null)
            {
                return false; // Or throw NotFoundException
            }

            // Validate LocationId exists if it's being changed (or always validate)
            // Validate LocationId exists if it's being changed
            if (roomToUpdate.LocationId != updateRoomDto.LocationId)
            {
                // Note: CancellationToken is not passed to the repository based on IGenericRepository
                var locationExists = await _locationRepository.GetByIdAsync(updateRoomDto.LocationId);
                if (locationExists == null)
                {
                    throw new ValidationException($"Location with ID {updateRoomDto.LocationId} does not exist.");
                }
            }

            _mapper.Map(updateRoomDto, roomToUpdate); // Update existing entity
            await _roomRepository.UpdateAsync(roomToUpdate); // Mark as updated
            // Assuming SaveChanges is handled by Unit of Work or DbContext elsewhere

            return true;
        }

        public async Task<bool> DeleteRoomAsync(int id, CancellationToken cancellationToken = default)
        {
            // Note: CancellationToken is not passed to the repository based on IGenericRepository
            var roomToDelete = await _roomRepository.GetByIdAsync(id);
            if (roomToDelete == null)
            {
                return false; // Or throw NotFoundException
            }

            await _roomRepository.DeleteAsync(roomToDelete);
            // Assuming SaveChanges is handled by Unit of Work or DbContext elsewhere

            return true;
        }
    }
}