using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces.Persistence
{
    public interface IParticipantRepository : IGenericRepository<Participant>
    {
        Task<Participant?> GetParticipantByEmailAsync(string email);


        Task<IReadOnlyList<Event>> GetEventsForParticipantAsync(int participantId);

    }
}