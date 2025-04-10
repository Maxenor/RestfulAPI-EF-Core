using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces.Persistence
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<IReadOnlyList<Rating>> GetRatingsBySessionIdAsync(int sessionId);

        Task<IReadOnlyList<Rating>> GetRatingsByParticipantIdAsync(int participantId);

        Task<double?> GetAverageRatingForSessionAsync(int sessionId);

    }
}