using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces.Persistence
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IReadOnlyList<Session>> GetSessionsByEventIdAsync(int eventId);

        Task<Session?> GetSessionWithDetailsAsync(int sessionId);
    }
}