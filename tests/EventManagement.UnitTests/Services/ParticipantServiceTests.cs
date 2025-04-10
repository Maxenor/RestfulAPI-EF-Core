using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventManagement.Application.DTOs.Common;
using EventManagement.Application.DTOs.Event;
using EventManagement.Application.DTOs.Participant;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Services;
using EventManagement.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace EventManagement.UnitTests.Services
{
    public class ParticipantServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IParticipantRepository> _mockParticipantRepository;
        private readonly ParticipantService _participantService;

        public ParticipantServiceTests()
        {
            // Setup mocks
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockParticipantRepository = new Mock<IParticipantRepository>();

            // Setup UnitOfWork to return our repositories
            _mockUnitOfWork.Setup(uow => uow.Participants).Returns(_mockParticipantRepository.Object);

            // Create instance of service to test
            _participantService = new ParticipantService(_mockMapper.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetParticipantsAsync_ShouldReturnPagedResultOfParticipants()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            var mockParticipants = new List<Participant>
            {
                new Participant
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Company = "ABC Corp",
                    JobTitle = "Developer"
                },
                new Participant
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Company = "XYZ Inc",
                    JobTitle = "Manager"
                }
            };

            var expectedDtos = new List<ParticipantDto>
            {
                new ParticipantDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new ParticipantDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
            };

            _mockParticipantRepository
                .Setup(repo => repo.ListAllAsync())
                .ReturnsAsync(mockParticipants);

            _mockMapper
                .Setup(m => m.Map<List<ParticipantDto>>(It.IsAny<List<Participant>>()))
                .Returns(expectedDtos);

            // Act
            var result = await _participantService.GetParticipantsAsync(pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.PageNumber.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalPages.Should().Be(1);
        }

        [Fact]
        public async Task GetParticipantByIdAsync_WithValidId_ShouldReturnParticipant()
        {
            // Arrange
            var participantId = 1;
            var mockParticipant = new Participant
            {
                Id = participantId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Company = "ABC Corp",
                JobTitle = "Developer"
            };

            var expectedDto = new ParticipantDto
            {
                Id = participantId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Company = "ABC Corp",
                JobTitle = "Developer"
            };

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .ReturnsAsync(mockParticipant);

            _mockMapper
                .Setup(m => m.Map<ParticipantDto>(mockParticipant))
                .Returns(expectedDto);

            // Act
            var result = await _participantService.GetParticipantByIdAsync(participantId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(participantId);
            result.FirstName.Should().Be("John");
            result.LastName.Should().Be("Doe");
            result.Email.Should().Be("john.doe@example.com");
        }

        [Fact]
        public async Task GetParticipantByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var participantId = 999; // Non-existent ID

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .Returns(Task.FromResult<Participant?>(null));

            // Act
            var result = await _participantService.GetParticipantByIdAsync(participantId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateParticipantAsync_WithValidData_ShouldCreateAndReturnParticipant()
        {
            // Arrange
            var createParticipantDto = new CreateParticipantDto
            {
                FirstName = "New",
                LastName = "Participant",
                Email = "new.participant@example.com",
                Company = "Test Company",
                JobTitle = "Test Role"
            };

            var newParticipant = new Participant
            {
                Id = 1,
                FirstName = "New",
                LastName = "Participant",
                Email = "new.participant@example.com",
                Company = "Test Company",
                JobTitle = "Test Role"
            };

            var expectedDto = new ParticipantDto
            {
                Id = 1,
                FirstName = "New",
                LastName = "Participant",
                Email = "new.participant@example.com",
                Company = "Test Company",
                JobTitle = "Test Role"
            };

            _mockParticipantRepository
                .Setup(repo => repo.GetParticipantByEmailAsync(createParticipantDto.Email))
                .Returns(Task.FromResult<Participant?>(null));

            _mockMapper
                .Setup(m => m.Map<Participant>(createParticipantDto))
                .Returns(newParticipant);

            _mockParticipantRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Participant>()))
                .ReturnsAsync(newParticipant);

            _mockMapper
                .Setup(m => m.Map<ParticipantDto>(newParticipant))
                .Returns(expectedDto);

            // Act
            var result = await _participantService.CreateParticipantAsync(createParticipantDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.FirstName.Should().Be("New");
            result.LastName.Should().Be("Participant");
            result.Email.Should().Be("new.participant@example.com");

            // Verify that transaction methods were called
            _mockUnitOfWork.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once); // Explicitly pass default CancellationToken
            _mockUnitOfWork.Verify(uow => uow.CommitTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateParticipantAsync_WithDuplicateEmail_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var createParticipantDto = new CreateParticipantDto
            {
                FirstName = "New",
                LastName = "Participant",
                Email = "existing.email@example.com",
                Company = "Test Company",
                JobTitle = "Test Role"
            };

            var existingParticipant = new Participant
            {
                Id = 5,
                FirstName = "Existing",
                LastName = "User",
                Email = "existing.email@example.com"
            };

            _mockParticipantRepository
                .Setup(repo => repo.GetParticipantByEmailAsync(createParticipantDto.Email))
                .ReturnsAsync(existingParticipant);

            // Act
            Func<Task> act = async () => await _participantService.CreateParticipantAsync(createParticipantDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Participant with email {createParticipantDto.Email} already exists.");

            // Verify that transaction was rolled back
            _mockUnitOfWork.Verify(uow => uow.RollbackTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task GetParticipantEventHistoryAsync_WithValidId_ShouldReturnEventsList()
        {
            // Arrange
            var participantId = 1;
            var mockEvents = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Test Event 1",
                    StartDate = new DateTime(2025, 4, 15),
                    EndDate = new DateTime(2025, 4, 16),
                    Status = EventStatus.Published
                },
                new Event
                {
                    Id = 2,
                    Title = "Test Event 2",
                    StartDate = new DateTime(2025, 5, 20),
                    EndDate = new DateTime(2025, 5, 21),
                    Status = EventStatus.Published
                }
            };

            var expectedDtos = new List<EventListDto>
            {
                new EventListDto { Id = 1, Title = "Test Event 1" },
                new EventListDto { Id = 2, Title = "Test Event 2" }
            };

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .ReturnsAsync(new Participant { Id = participantId });

            _mockParticipantRepository
                .Setup(repo => repo.GetEventsForParticipantAsync(participantId))
                .ReturnsAsync(mockEvents);

            _mockMapper
                .Setup(m => m.Map<List<EventListDto>>(mockEvents))
                .Returns(expectedDtos);

            // Act
            var result = await _participantService.GetParticipantEventHistoryAsync(participantId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(1);
            result[0].Title.Should().Be("Test Event 1");
            result[1].Id.Should().Be(2);
            result[1].Title.Should().Be("Test Event 2");
        }

        [Fact]
        public async Task GetParticipantEventHistoryAsync_WithInvalidId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var participantId = 999; // Non-existent ID

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .Returns(Task.FromResult<Participant?>(null));

            // Act
            Func<Task> act = async () => await _participantService.GetParticipantEventHistoryAsync(participantId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Participant with ID {participantId} not found.");
        }

        [Fact]
        public async Task DeleteParticipantAsync_WithInvalidId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var participantId = 999; // Non-existent ID

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .Returns(Task.FromResult<Participant?>(null));

            // Act
            Func<Task> act = async () => await _participantService.DeleteParticipantAsync(participantId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Participant with ID {participantId} not found.");

            // Verify transaction was rolled back
            _mockUnitOfWork.Verify(uow => uow.RollbackTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteParticipantAsync_WithRegisteredParticipant_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var participantId = 1;
            var mockParticipant = new Participant
            {
                Id = participantId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var mockEvents = new List<Event>
            {
                new Event
                {
                    Id = 1,
                    Title = "Test Event 1"
                }
            };

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .ReturnsAsync(mockParticipant);

            _mockParticipantRepository
                .Setup(repo => repo.GetEventsForParticipantAsync(participantId))
                .ReturnsAsync(mockEvents);

            // Act
            Func<Task> act = async () => await _participantService.DeleteParticipantAsync(participantId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Cannot delete participant {participantId} as they are registered for one or more events.");

            // Verify transaction was rolled back
            _mockUnitOfWork.Verify(uow => uow.RollbackTransactionAsync(), Times.Once);
        }

        // --- UpdateParticipantAsync Tests ---

        [Fact]
        public async Task UpdateParticipantAsync_WithValidData_ShouldUpdateParticipant()
        {
            // Arrange
            var participantId = 1;
            var updateDto = new UpdateParticipantDto
            {
                Id = participantId,
                FirstName = "UpdatedJohn",
                LastName = "UpdatedDoe",
                Email = "updated.john.doe@example.com",
                Company = "Updated ABC Corp",
                JobTitle = "Senior Developer"
            };

            var existingParticipant = new Participant
            {
                Id = participantId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Company = "ABC Corp",
                JobTitle = "Developer"
            };

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .ReturnsAsync(existingParticipant);

            // Assume email is changing, mock the check for the new email (return null as it's unique)
            _mockParticipantRepository
                .Setup(repo => repo.GetParticipantByEmailAsync(updateDto.Email))
                .Returns(Task.FromResult<Participant?>(null));

            _mockMapper.Setup(m => m.Map(updateDto, existingParticipant)); // Mock the mapping action
            _mockParticipantRepository.Setup(repo => repo.UpdateAsync(existingParticipant)); // Mock update
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(default)).ReturnsAsync(1); // Mock save

            // Act
            await _participantService.UpdateParticipantAsync(updateDto);

            // Assert
            _mockParticipantRepository.Verify(repo => repo.GetByIdAsync(participantId), Times.Once);
            _mockParticipantRepository.Verify(repo => repo.GetParticipantByEmailAsync(updateDto.Email), Times.Once); // Verify email check
            _mockMapper.Verify(m => m.Map(updateDto, existingParticipant), Times.Once);
            _mockParticipantRepository.Verify(repo => repo.UpdateAsync(existingParticipant), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.RollbackTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateParticipantAsync_WithNonExistentId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var participantId = 999;
            var updateDto = new UpdateParticipantDto { Id = participantId, Email = "test@test.com" };

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantId))
                .Returns(Task.FromResult<Participant?>(null)); // Simulate not found

            // Act
            Func<Task> act = async () => await _participantService.UpdateParticipantAsync(updateDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage($"Participant with ID {participantId} not found.");

            _mockUnitOfWork.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitTransactionAsync(), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.RollbackTransactionAsync(), Times.Once);
            _mockParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Participant>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task UpdateParticipantAsync_WithDuplicateEmail_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var participantIdToUpdate = 1;
            var existingParticipantWithEmailId = 2;
            var duplicateEmail = "duplicate.email@example.com";

            var updateDto = new UpdateParticipantDto
            {
                Id = participantIdToUpdate,
                FirstName = "UpdatedJohn",
                LastName = "UpdatedDoe",
                Email = duplicateEmail // Attempting to update to an existing email
            };

            var participantToUpdate = new Participant
            {
                Id = participantIdToUpdate,
                FirstName = "John",
                LastName = "Doe",
                Email = "original.john.doe@example.com" // Original email
            };

            var existingParticipantWithDuplicateEmail = new Participant
            {
                Id = existingParticipantWithEmailId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = duplicateEmail // The email we are trying to update to
            };

            _mockParticipantRepository
                .Setup(repo => repo.GetByIdAsync(participantIdToUpdate))
                .ReturnsAsync(participantToUpdate);

            // Mock the email check to return the *other* participant with the same email
            _mockParticipantRepository
                .Setup(repo => repo.GetParticipantByEmailAsync(duplicateEmail))
                .ReturnsAsync(existingParticipantWithDuplicateEmail);

            // Act
            Func<Task> act = async () => await _participantService.UpdateParticipantAsync(updateDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage($"Another participant with email {duplicateEmail} already exists.");

            _mockUnitOfWork.Verify(uow => uow.BeginTransactionAsync(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitTransactionAsync(), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.RollbackTransactionAsync(), Times.Once);
            _mockParticipantRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Participant>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(default), Times.Never);
        }
    }
}