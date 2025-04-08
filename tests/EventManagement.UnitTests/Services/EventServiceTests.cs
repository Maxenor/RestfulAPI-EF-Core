using AutoMapper;
using Moq;
using FluentAssertions;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Services;
using EventManagement.Domain.Entities;
using EventManagement.Application.DTOs.Event;
using Xunit;
using System.Linq.Expressions;
using EventManagement.Application.Exceptions;

namespace EventManagement.UnitTests.Services;

public class EventServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EventService _eventService;

    public EventServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockEventRepository = new Mock<IEventRepository>();
        _mockMapper = new Mock<IMapper>();

        // Setup UnitOfWork to return the mocked repository
        _mockUnitOfWork.Setup(uow => uow.Events).Returns(_mockEventRepository.Object);

        _eventService = new EventService(_mockMapper.Object, _mockUnitOfWork.Object); // Corrected constructor argument order
    }

    // --- GetEventByIdAsync Tests ---

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEvent_WhenEventExists()
    {
        // Arrange
        var eventId = 1;
        var eventEntity = new Event { Id = eventId, Title = "Test Event" };
        var eventDto = new EventDetailDto { Id = eventId, Title = "Test Event" };

        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(eventEntity);
        _mockMapper.Setup(m => m.Map<EventDetailDto>(eventEntity)).Returns(eventDto);

        // Act
        var result = await _eventService.GetEventByIdAsync(eventId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(eventDto);
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockMapper.Verify(m => m.Map<EventDetailDto>(eventEntity), Times.Once);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = 1;
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

        // Act
        Func<Task> act = async () => await _eventService.GetEventByIdAsync(eventId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"Event with ID {eventId} not found.");
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockMapper.Verify(m => m.Map<EventDetailDto>(It.IsAny<Event>()), Times.Never);
    }

    // --- CreateEventAsync Tests ---

    [Fact]
    public async Task CreateEventAsync_ShouldReturnCreatedEventDto_WhenSuccessful()
    {
        // Arrange
        var createEventDto = new CreateEventDto { Title = "New Event", StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(2) };
        var eventEntity = new Event { Id = 0, Title = "New Event" }; // Id is 0 before Add
        var createdEventEntity = new Event { Id = 1, Title = "New Event" }; // Id is 1 after Add
        var eventDetailDto = new EventDetailDto { Id = 1, Title = "New Event" };

        _mockMapper.Setup(m => m.Map<Event>(createEventDto)).Returns(eventEntity);
        _mockEventRepository.Setup(repo => repo.AddAsync(eventEntity)).Callback<Event>(e => e.Id = 1); // Simulate ID generation
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(default)).ReturnsAsync(1); // Use SaveChangesAsync
        _mockMapper.Setup(m => m.Map<EventDetailDto>(eventEntity)).Returns(eventDetailDto); // Map the entity *after* ID is set

        // Act
        var result = await _eventService.CreateEventAsync(createEventDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(eventDetailDto);
        _mockMapper.Verify(m => m.Map<Event>(createEventDto), Times.Once);
        _mockEventRepository.Verify(repo => repo.AddAsync(It.Is<Event>(e => e.Title == createEventDto.Title)), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once); // Verify SaveChangesAsync
        _mockMapper.Verify(m => m.Map<EventDetailDto>(It.Is<Event>(e => e.Id == 1)), Times.Once);
    }

    // --- UpdateEventAsync Tests ---
    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateEvent_WhenEventExists()
    {
        // Arrange
        var eventId = 1;
        var updateEventDto = new UpdateEventDto { Id = eventId, Title = "Updated Event", Description = "Updated Desc" }; // Add Id to DTO
        var existingEvent = new Event { Id = eventId, Title = "Old Title", Description = "Old Desc" };

        _mockEventRepository.Setup(repo => repo.GetByIdAsync(updateEventDto.Id)).ReturnsAsync(existingEvent); // Use Id from DTO
        _mockMapper.Setup(m => m.Map(updateEventDto, existingEvent)).Callback<UpdateEventDto, Event>((dto, entity) => {
            entity.Title = dto.Title; // Simulate mapping
            entity.Description = dto.Description;
        });
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(default)).ReturnsAsync(1); // Use SaveChangesAsync

        // Act
        await _eventService.UpdateEventAsync(updateEventDto); // Pass only DTO

        // Assert
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(updateEventDto.Id), Times.Once); // Verify using Id from DTO
        _mockMapper.Verify(m => m.Map(updateEventDto, existingEvent), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once); // Verify SaveChangesAsync
        existingEvent.Title.Should().Be(updateEventDto.Title); // Verify changes were applied
        existingEvent.Description.Should().Be(updateEventDto.Description);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = 1; // Keep eventId for NotFound check
        var updateEventDto = new UpdateEventDto { Id = eventId, Title = "Updated Event" }; // Add Id to DTO
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(updateEventDto.Id)).ReturnsAsync((Event)null); // Use Id from DTO

        // Act
        Func<Task> act = async () => await _eventService.UpdateEventAsync(updateEventDto); // Pass only DTO

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"Event with ID {updateEventDto.Id} not found."); // Use Id from DTO in message
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(updateEventDto.Id), Times.Once); // Verify using Id from DTO
        _mockMapper.Verify(m => m.Map(It.IsAny<UpdateEventDto>(), It.IsAny<Event>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Never); // Verify SaveChangesAsync
    }


    // --- DeleteEventAsync Tests ---

    [Fact]
    public async Task DeleteEventAsync_ShouldDeleteEvent_WhenEventExists()
    {
        // Arrange
        var eventId = 1;
        var existingEvent = new Event { Id = eventId, Title = "Test Event" };

        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(default)).ReturnsAsync(1); // Use SaveChangesAsync

        // Act
        await _eventService.DeleteEventAsync(eventId);

        // Assert
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockEventRepository.Verify(repo => repo.DeleteAsync(existingEvent), Times.Once); // Use DeleteAsync
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once); // Verify SaveChangesAsync
    }

    [Fact]
    public async Task DeleteEventAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = 1;
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync((Event)null);

        // Act
        Func<Task> act = async () => await _eventService.DeleteEventAsync(eventId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"Event with ID {eventId} not found.");
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockEventRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Event>()), Times.Never); // Use DeleteAsync
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Never); // Verify SaveChangesAsync
    }

    // TODO: Add tests for GetAllEventsAsync (including filtering and pagination)
    // TODO: Add tests for AddParticipantToEventAsync
    // TODO: Add tests for RemoveParticipantFromEventAsync
    // TODO: Add tests for GetParticipantsForEventAsync
}