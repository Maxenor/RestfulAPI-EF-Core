using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces.Persistence
{
    public interface ISpeakerRepository : IGenericRepository<Speaker>
    {
        Task<Speaker?> GetSpeakerByEmailAsync(string email);

        Task<IReadOnlyList<Speaker>> GetSpeakersForSessionAsync(int sessionId);
    }
}