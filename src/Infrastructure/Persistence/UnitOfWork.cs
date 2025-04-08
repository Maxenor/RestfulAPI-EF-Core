using EventManagement.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence
{
    /// <summary>
    /// Implementation of the IUnitOfWork interface for managing transactions across repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EventManagementDbContext _dbContext;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        // Repository fields - using lazy loading for better performance
        private ICategoryRepository _categoryRepository;
        private IEventRepository _eventRepository;
        private ILocationRepository _locationRepository;
        private IParticipantRepository _participantRepository;
        private IRatingRepository _ratingRepository;
        private IRoomRepository _roomRepository;
        private ISessionRepository _sessionRepository;
        private ISpeakerRepository _speakerRepository;

        public UnitOfWork(
            EventManagementDbContext dbContext,
            ICategoryRepository categoryRepository,
            IEventRepository eventRepository,
            ILocationRepository locationRepository,
            IParticipantRepository participantRepository,
            IRatingRepository ratingRepository,
            IRoomRepository roomRepository, 
            ISessionRepository sessionRepository,
            ISpeakerRepository speakerRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
            _participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
            _ratingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(ratingRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
            _speakerRepository = speakerRepository ?? throw new ArgumentNullException(nameof(speakerRepository));
        }

        // Repository properties
        public ICategoryRepository Categories => _categoryRepository;
        public IEventRepository Events => _eventRepository;
        public ILocationRepository Locations => _locationRepository;
        public IParticipantRepository Participants => _participantRepository;
        public IRatingRepository Ratings => _ratingRepository;
        public IRoomRepository Rooms => _roomRepository;
        public ISessionRepository Sessions => _sessionRepository;
        public ISpeakerRepository Speakers => _speakerRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            // Start a new transaction if one isn't already in progress
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                // Commit the transaction if one exists
                await _dbContext.SaveChangesAsync();
                
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _transaction?.Dispose();
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }
    }
}