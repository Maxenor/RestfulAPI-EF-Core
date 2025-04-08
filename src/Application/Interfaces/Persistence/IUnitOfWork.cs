using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Interface for managing transactions and coordinating work across multiple repositories
    /// </summary>
    public interface IUnitOfWork
    {
        // Repository properties
        ICategoryRepository Categories { get; }
        IEventRepository Events { get; }
        ILocationRepository Locations { get; }
        IParticipantRepository Participants { get; }
        IRatingRepository Ratings { get; }
        IRoomRepository Rooms { get; }
        ISessionRepository Sessions { get; }
        ISpeakerRepository Speakers { get; }

        /// <summary>
        /// Saves all changes made in this unit of work to the database
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete</param>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Begins a transaction on the database context
        /// </summary>
        Task BeginTransactionAsync();
        
        /// <summary>
        /// Commits the transaction on the database context
        /// </summary>
        Task CommitTransactionAsync();
        
        /// <summary>
        /// Rolls back the transaction on the database context
        /// </summary>
        Task RollbackTransactionAsync();
    }
}