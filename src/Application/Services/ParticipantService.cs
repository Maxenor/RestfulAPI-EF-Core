using AutoMapper;
using EventManagement.Application.DTOs.Common;
using EventManagement.Application.DTOs.Event;
using EventManagement.Application.DTOs.Participant;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ParticipantService(
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<PagedResult<ParticipantDto>> GetParticipantsAsync(int pageNumber, int pageSize)
        {
            var participants = await _unitOfWork.Participants.ListAllAsync();
            var participantsOrdered = participants
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalCount = participants.Count;
            var participantDtos = _mapper.Map<List<ParticipantDto>>(participantsOrdered);

            return PagedResult<ParticipantDto>.CreateWithKnownTotalCount(participantDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<ParticipantDto?> GetParticipantByIdAsync(int id)
        {
            var participant = await _unitOfWork.Participants.GetByIdAsync(id);
            if (participant == null)
            {
                return null;
            }
            return _mapper.Map<ParticipantDto>(participant);
        }

        public async Task<ParticipantDto> CreateParticipantAsync(CreateParticipantDto createParticipantDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existingParticipant = await _unitOfWork.Participants.GetParticipantByEmailAsync(createParticipantDto.Email);
                if (existingParticipant != null)
                {
                    throw new InvalidOperationException($"Participant with email {createParticipantDto.Email} already exists.");
                }

                var participantEntity = _mapper.Map<Participant>(createParticipantDto);

                var createdParticipant = await _unitOfWork.Participants.AddAsync(participantEntity);
                await _unitOfWork.SaveChangesAsync();
                
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<ParticipantDto>(createdParticipant);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateParticipantAsync(UpdateParticipantDto updateParticipantDto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var participantToUpdate = await _unitOfWork.Participants.GetByIdAsync(updateParticipantDto.Id);
                if (participantToUpdate == null)
                {
                    throw new InvalidOperationException($"Participant with ID {updateParticipantDto.Id} not found.");
                }

                if (!participantToUpdate.Email.Equals(updateParticipantDto.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingParticipant = await _unitOfWork.Participants.GetParticipantByEmailAsync(updateParticipantDto.Email);
                    if (existingParticipant != null && existingParticipant.Id != updateParticipantDto.Id)
                    {
                        throw new InvalidOperationException($"Another participant with email {updateParticipantDto.Email} already exists.");
                    }
                }

                _mapper.Map(updateParticipantDto, participantToUpdate);

                await _unitOfWork.Participants.UpdateAsync(participantToUpdate);
                await _unitOfWork.SaveChangesAsync();
                
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteParticipantAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var participantToDelete = await _unitOfWork.Participants.GetByIdAsync(id);
                if (participantToDelete == null)
                {
                    throw new InvalidOperationException($"Participant with ID {id} not found.");
                }

                var events = await _unitOfWork.Participants.GetEventsForParticipantAsync(id);
                bool isRegistered = events.Any();
                
                if (isRegistered)
                {
                    throw new InvalidOperationException($"Cannot delete participant {id} as they are registered for one or more events.");
                }

                await _unitOfWork.Participants.DeleteAsync(participantToDelete);
                await _unitOfWork.SaveChangesAsync();
                
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<EventListDto>> GetParticipantEventHistoryAsync(int participantId)
        {
            var participantExists = await _unitOfWork.Participants.GetByIdAsync(participantId) != null;
            if (!participantExists)
            {
                throw new InvalidOperationException($"Participant with ID {participantId} not found.");
            }

            var events = await _unitOfWork.Participants.GetEventsForParticipantAsync(participantId);
            return _mapper.Map<List<EventListDto>>(events);
        }
    }
}