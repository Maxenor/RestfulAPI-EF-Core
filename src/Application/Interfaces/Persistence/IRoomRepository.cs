using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces.Persistence
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<IReadOnlyList<Room>> GetRoomsByLocationIdAsync(int locationId);

    }
}