using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces.Persistence
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<Event?> GetEventWithDetailsAsync(int id);

        Task<IReadOnlyList<Event>> FindEventsAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? locationId,
            int? categoryId,
            EventStatus? status,
            int pageNumber,
            int pageSize);

         Task<int> GetTotalEventCountAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? locationId,
            int? categoryId,
            EventStatus? status);
    }
}