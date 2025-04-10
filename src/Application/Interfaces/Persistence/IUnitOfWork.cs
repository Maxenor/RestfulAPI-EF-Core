namespace EventManagement.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IEventRepository Events { get; }
        ILocationRepository Locations { get; }
        IParticipantRepository Participants { get; }
        IRatingRepository Ratings { get; }
        IRoomRepository Rooms { get; }
        ISessionRepository Sessions { get; }
        ISpeakerRepository Speakers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        Task BeginTransactionAsync();
        
        Task CommitTransactionAsync();
        
        Task RollbackTransactionAsync();
    }
}