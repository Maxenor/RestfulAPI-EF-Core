using AutoMapper;
using Moq;
using FluentAssertions;
using EventManagement.Application.DTOs.Category;
using EventManagement.Application.DTOs.Location;
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
private readonly Mock<ICategoryRepository> _mockCategoryRepository; // Added for FK checks
private readonly Mock<ILocationRepository> _mockLocationRepository; // Added for FK checks

public EventServiceTests()
{
   _mockUnitOfWork = new Mock<IUnitOfWork>();
   _mockEventRepository = new Mock<IEventRepository>();
   _mockCategoryRepository = new Mock<ICategoryRepository>(); // Added
   _mockLocationRepository = new Mock<ILocationRepository>(); // Added
   _mockMapper = new Mock<IMapper>();

   // Setup UnitOfWork to return the mocked repositories
   _mockUnitOfWork.Setup(uow => uow.Events).Returns(_mockEventRepository.Object);
   _mockUnitOfWork.Setup(uow => uow.Categories).Returns(_mockCategoryRepository.Object); // Added
   _mockUnitOfWork.Setup(uow => uow.Locations).Returns(_mockLocationRepository.Object); // Added

   _eventService = new EventService(_mockMapper.Object, _mockUnitOfWork.Object);
}
// Removed extra closing brace

    // --- GetEventByIdAsync Tests ---

    [Fact]
    public async Task GetEventByIdAsync_ShouldReturnEvent_WhenEventExists()
    {
        // Arrange
        var eventId = 1;
        var eventEntity = new Event { Id = eventId, Title = "Test Event" };
        var eventDto = new EventDetailDto { Id = eventId, Title = "Test Event" };

       // Service uses GetEventWithDetailsAsync internally for GetByIdAsync
       _mockEventRepository.Setup(repo => repo.GetEventWithDetailsAsync(eventId)).ReturnsAsync(eventEntity);
       _mockMapper.Setup(m => m.Map<EventDetailDto>(eventEntity)).Returns(eventDto);

        // Act
        var result = await _eventService.GetEventByIdAsync(eventId);

        // Assert
        result.Should().NotBeNull();
       result.Should().BeEquivalentTo(eventDto);
       _mockEventRepository.Verify(repo => repo.GetEventWithDetailsAsync(eventId), Times.Once); // Verify correct method
       _mockMapper.Verify(m => m.Map<EventDetailDto>(eventEntity), Times.Once);
    }

    [Fact]
    public async Task GetEventByIdAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = 1;
        _mockEventRepository.Setup(repo => repo.GetEventWithDetailsAsync(eventId)).Returns(Task.FromResult<Event?>(null)); // Service uses GetEventWithDetailsAsync

        // Act
        Func<Task> act = async () => await _eventService.GetEventByIdAsync(eventId);

        // Assert
       await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Entity \"Event\" (*) was not found."); // Corrected message format
       _mockEventRepository.Verify(repo => repo.GetEventWithDetailsAsync(eventId), Times.Once); // Verify correct method
        _mockMapper.Verify(m => m.Map<EventDetailDto>(It.IsAny<Event>()), Times.Never);
    }

    // --- CreateEventAsync Tests ---

    [Fact]
    public async Task CreateEventAsync_ShouldReturnCreatedEventDto_WhenSuccessful()
    {
        // Arrange
       var createEventDto = new CreateEventDto { Title = "New Event", StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(2), CategoryId = 1, LocationId = 1 };
       var eventEntity = new Event { Id = 0, Title = "New Event", CategoryId = 1, LocationId = 1 }; // Id is 0 before Add
       var createdEventEntity = new Event { Id = 1, Title = "New Event", CategoryId = 1, LocationId = 1 }; // Id is 1 after Add
      var eventDetailDto = new EventDetailDto { Id = 1, Title = "New Event", Category = new CategoryDto { Id = 1, Name = "Test Category" }, Location = new LocationDto { Id = 1, Name = "Test Location" } }; // Corrected DTO structure
       var categoryEntity = new Category { Id = 1, Name = "Test Category" };
       var locationEntity = new Location { Id = 1, Name = "Test Location" };

       // Mock FK checks
       _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(createEventDto.CategoryId)).ReturnsAsync(categoryEntity);
       _mockLocationRepository.Setup(repo => repo.GetByIdAsync(createEventDto.LocationId)).ReturnsAsync(locationEntity);

       _mockMapper.Setup(m => m.Map<Event>(createEventDto)).Returns(eventEntity);
       _mockEventRepository.Setup(repo => repo.AddAsync(eventEntity)).ReturnsAsync(createdEventEntity); // Return the entity with ID
       _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(default)).ReturnsAsync(1);

       // Mock the GetEventByIdAsync call made *within* CreateEventAsync
       _mockEventRepository.Setup(repo => repo.GetEventWithDetailsAsync(createdEventEntity.Id)).ReturnsAsync(createdEventEntity);
       _mockMapper.Setup(m => m.Map<EventDetailDto>(createdEventEntity)).Returns(eventDetailDto); // Map the final entity

        // Act
        var result = await _eventService.CreateEventAsync(createEventDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(eventDetailDto);
        _mockMapper.Verify(m => m.Map<Event>(createEventDto), Times.Once);
        _mockEventRepository.Verify(repo => repo.AddAsync(It.Is<Event>(e => e.Title == createEventDto.Title)), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once); // Verify SaveChangesAsync
       _mockMapper.Verify(m => m.Map<EventDetailDto>(createdEventEntity), Times.Once); // Verify mapping the final entity
    }

    // --- UpdateEventAsync Tests ---
    [Fact]
    public async Task UpdateEventAsync_ShouldUpdateEvent_WhenEventExists()
    {
        // Arrange
        var eventId = 1;
      // var eventId = 1; // Removed duplicate declaration, Id comes from updateEventDto
       var updateEventDto = new UpdateEventDto { Id = eventId, Title = "Updated Event", Description = "Updated Desc", CategoryId = 2, LocationId = 2, StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(2) };
       var existingEvent = new Event { Id = eventId, Title = "Old Title", Description = "Old Desc", CategoryId = 1, LocationId = 1 };
       var categoryEntity = new Category { Id = 2, Name = "Updated Category" };
       var locationEntity = new Location { Id = 2, Name = "Updated Location" };

       _mockEventRepository.Setup(repo => repo.GetByIdAsync(updateEventDto.Id)).ReturnsAsync(existingEvent);

       // Mock FK checks
       _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(updateEventDto.CategoryId)).ReturnsAsync(categoryEntity);
       _mockLocationRepository.Setup(repo => repo.GetByIdAsync(updateEventDto.LocationId)).ReturnsAsync(locationEntity);

       _mockMapper.Setup(m => m.Map(updateEventDto, existingEvent)).Callback<UpdateEventDto, Event>((dto, entity) => {
           entity.Title = dto.Title; // Simulate mapping
           entity.Description = dto.Description;
           entity.CategoryId = dto.CategoryId;
           entity.LocationId = dto.LocationId;
           entity.StartDate = dto.StartDate;
           entity.EndDate = dto.EndDate;
       });
       _mockEventRepository.Setup(repo => repo.UpdateAsync(existingEvent)).Returns(Task.CompletedTask); // Mock UpdateAsync
       _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        await _eventService.UpdateEventAsync(updateEventDto); // Pass only DTO

        // Assert
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(updateEventDto.Id), Times.Once); // Verify using Id from DTO
        _mockMapper.Verify(m => m.Map(updateEventDto, existingEvent), Times.Once);
       _mockEventRepository.Verify(repo => repo.UpdateAsync(existingEvent), Times.Once); // Verify UpdateAsync call
       _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once);
       existingEvent.Title.Should().Be(updateEventDto.Title);
       existingEvent.Description.Should().Be(updateEventDto.Description);
       existingEvent.CategoryId.Should().Be(updateEventDto.CategoryId);
       existingEvent.LocationId.Should().Be(updateEventDto.LocationId);
    }

    [Fact]
    public async Task UpdateEventAsync_ShouldThrowNotFoundException_WhenEventDoesNotExist()
    {
        // Arrange
        var eventId = 1; // Keep eventId for NotFound check
        var updateEventDto = new UpdateEventDto { Id = eventId, Title = "Updated Event" }; // Add Id to DTO
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(updateEventDto.Id)).Returns(Task.FromResult<Event?>(null)); // Use Id from DTO

        // Act
        Func<Task> act = async () => await _eventService.UpdateEventAsync(updateEventDto); // Pass only DTO

        // Assert
       await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Entity \"Event\" (*) was not found."); // Corrected message format
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
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(eventId)).Returns(Task.FromResult<Event?>(null));

        // Act
        Func<Task> act = async () => await _eventService.DeleteEventAsync(eventId);

        // Assert
       await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Entity \"Event\" (*) was not found."); // Corrected message format
        _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        _mockEventRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Event>()), Times.Never); // Use DeleteAsync
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Never); // Verify SaveChangesAsync
    }

    // TODO: Add tests for GetAllEventsAsync (including filtering and pagination)
    // TODO: Add tests for AddParticipantToEventAsync
    // TODO: Add tests for RemoveParticipantFromEventAsync
    // TODO: Add tests for GetParticipantsForEventAsync
}