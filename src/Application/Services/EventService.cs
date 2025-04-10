using AutoMapper;
using EventManagement.Application.DTOs.Common;
using EventManagement.Application.DTOs.Event;
using EventManagement.Application.Exceptions;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;


namespace EventManagement.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EventService(
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<PagedResult<EventListDto>> GetEventsAsync(
            DateTime? startDate, DateTime? endDate, int? locationId, int? categoryId, EventStatus? status,
            int pageNumber, int pageSize)
        {
            var events = await _unitOfWork.Events.FindEventsAsync(startDate, endDate, locationId, categoryId, status, pageNumber, pageSize);
            var totalCount = await _unitOfWork.Events.GetTotalEventCountAsync(startDate, endDate, locationId, categoryId, status);

            var eventDtos = _mapper.Map<List<EventListDto>>(events);

            return PagedResult<EventListDto>.CreateWithKnownTotalCount(eventDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<EventDetailDto?> GetEventByIdAsync(int id) // Added nullable '?' to match interface
        {
            var eventEntity = await _unitOfWork.Events.GetEventWithDetailsAsync(id);
            if (eventEntity == null)
            {
                throw new NotFoundException(nameof(Event), id);
            }
            return _mapper.Map<EventDetailDto>(eventEntity)!;
        }

        public async Task<EventDetailDto> CreateEventAsync(CreateEventDto createEventDto)
        {
            // Start a transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Validate input data
                if (createEventDto.EndDate < createEventDto.StartDate)
                {
                    throw new ValidationException("EndDate", "End date must be after start date");
                }

                // Validate Foreign Keys exist
                var category = await _unitOfWork.Categories.GetByIdAsync(createEventDto.CategoryId);
                if (category == null)
                    throw new NotFoundException(nameof(Category), createEventDto.CategoryId);

                var location = await _unitOfWork.Locations.GetByIdAsync(createEventDto.LocationId);
                if (location == null)
                    throw new NotFoundException(nameof(Location), createEventDto.LocationId);

                var eventEntity = _mapper.Map<Event>(createEventDto);
                eventEntity.Status = EventStatus.Draft;

                var createdEvent = await _unitOfWork.Events.AddAsync(eventEntity);
                await _unitOfWork.SaveChangesAsync(); // Save changes
                
                // Commit the transaction
                await _unitOfWork.CommitTransactionAsync();

                // Fetch the details again to include navigation properties for the response DTO
                var detailedEvent = await GetEventByIdAsync(createdEvent.Id);
                return detailedEvent;
            }
            catch
            {
                // Rollback in case of any error
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateEventAsync(UpdateEventDto updateEventDto)
        {
            // Start a transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Validate input data
                if (updateEventDto.EndDate < updateEventDto.StartDate)
                {
                    throw new ValidationException("EndDate", "End date must be after start date");
                }
                
                var eventToUpdate = await _unitOfWork.Events.GetByIdAsync(updateEventDto.Id);
                if (eventToUpdate == null)
                {
                    throw new NotFoundException(nameof(Event), updateEventDto.Id);
                }

                // Validate Foreign Keys exist
                var category = await _unitOfWork.Categories.GetByIdAsync(updateEventDto.CategoryId);
                if (category == null)
                    throw new NotFoundException(nameof(Category), updateEventDto.CategoryId);

                var location = await _unitOfWork.Locations.GetByIdAsync(updateEventDto.LocationId);
                if (location == null)
                    throw new NotFoundException(nameof(Location), updateEventDto.LocationId);

                // Map updated fields onto the existing entity
                _mapper.Map(updateEventDto, eventToUpdate);

                await _unitOfWork.Events.UpdateAsync(eventToUpdate); // Marks entity as modified
                await _unitOfWork.SaveChangesAsync(); // Save changes
                
                // Commit the transaction
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                // Rollback the transaction in case of any error
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteEventAsync(int id)
        {
            // Start a transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var eventToDelete = await _unitOfWork.Events.GetByIdAsync(id);
                if (eventToDelete == null)
                {
                    throw new NotFoundException(nameof(Event), id);
                }

                if (eventToDelete.Status == EventStatus.Completed)
                {
                    throw new ConflictException($"Event with ID {id} is completed and cannot be deleted.");
                }

                await _unitOfWork.Events.DeleteAsync(eventToDelete);
                await _unitOfWork.SaveChangesAsync(); // Save changes
                
                // Commit the transaction
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                // Rollback the transaction in case of any error
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RegisterParticipantAsync(int eventId, int participantId)
        {
            // Start a transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Check if event exists
                var eventEntity = await _unitOfWork.Events.GetEventWithDetailsAsync(eventId);
                if (eventEntity == null)
                    throw new NotFoundException(nameof(Event), eventId);

                // Check if participant exists
                var participant = await _unitOfWork.Participants.GetByIdAsync(participantId);
                if (participant == null)
                    throw new NotFoundException(nameof(Participant), participantId);

                // Check if already registered
                bool isAlreadyRegistered = eventEntity.EventParticipants.Any(ep => ep.ParticipantId == participantId);
                if (isAlreadyRegistered)
                {
                    throw new ConflictException($"Participant {participantId} is already registered for event {eventId}.");
                }

                if (eventEntity.Status == EventStatus.Completed || eventEntity.Status == EventStatus.Cancelled)
                {
                    throw new ConflictException($"Cannot register for event {eventId} because it is {eventEntity.Status}.");
                }

                var registration = new EventParticipant
                {
                    EventId = eventId,
                    ParticipantId = participantId,
                    RegistrationDate = DateTime.UtcNow,
                    AttendanceStatus = AttendanceStatus.Registered
                };

                eventEntity.EventParticipants.Add(registration); // Add to collection
                await _unitOfWork.SaveChangesAsync();
                
                // Commit the transaction
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                // Rollback the transaction in case of any error
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UnregisterParticipantAsync(int eventId, int participantId)
        {
            // Start a transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Check if event exists
                var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
                if (eventEntity == null)
                    throw new NotFoundException(nameof(Event), eventId);

                // Find the specific registration record
                var eventWithParticipants = await _unitOfWork.Events.GetEventWithDetailsAsync(eventId);
                if (eventWithParticipants == null)
                    throw new NotFoundException(nameof(Event), eventId);
                    
                var registration = eventWithParticipants.EventParticipants.FirstOrDefault(ep => ep.ParticipantId == participantId);

                if (registration == null)
                {
                    throw new NotFoundException("Registration", $"Event {eventId}, Participant {participantId}");
                }

                if (eventWithParticipants.StartDate <= DateTime.UtcNow)
                {
                    throw new ConflictException($"Cannot unregister from event {eventId} as it has already started or completed.");
                }

                eventWithParticipants.EventParticipants.Remove(registration);
                await _unitOfWork.SaveChangesAsync();
                
                // Commit the transaction
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                // Rollback the transaction in case of any error
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}