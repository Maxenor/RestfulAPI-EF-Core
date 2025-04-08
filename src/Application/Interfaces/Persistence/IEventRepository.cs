using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface specific to the Event entity.
    /// Inherits common CRUD operations from IGenericRepository.
    /// </summary>
    public interface IEventRepository : IGenericRepository<Event>
    {
        /// <summary>
        /// Gets a single event by its ID, including related details like Category, Location, Sessions, etc.
        /// The specific related entities to include should be determined by the implementation.
        /// </summary>
        /// <param name="id">The event identifier.</param>
        /// <returns>The detailed event if found; otherwise, null.</returns>
        Task<Event?> GetEventWithDetailsAsync(int id);

        /// <summary>
        /// Finds events based on specified filter criteria and pagination parameters.
        /// </summary>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="locationId">Optional location filter.</param>
        /// <param name="categoryId">Optional category filter.</param>
        /// <param name="status">Optional status filter.</param>
        /// <param name="pageNumber">The page number to retrieve (1-based).</param>
        /// <param name="pageSize">The number of events per page.</param>
        /// <returns>A read-only list of events matching the criteria for the specified page.</returns>
        Task<IReadOnlyList<Event>> FindEventsAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? locationId,
            int? categoryId,
            EventStatus? status,
            int pageNumber,
            int pageSize);

        /// <summary>
        /// Gets the total count of events matching the specified filter criteria.
        /// Used for calculating total pages for pagination.
        /// </summary>
        /// <param name="startDate">Optional start date filter.</param>
        /// <param name="endDate">Optional end date filter.</param>
        /// <param name="locationId">Optional location filter.</param>
        /// <param name="categoryId">Optional category filter.</param>
        /// <param name="status">Optional status filter.</param>
        /// <returns>The total number of events matching the filters.</returns>
         Task<int> GetTotalEventCountAsync(
            DateTime? startDate,
            DateTime? endDate,
            int? locationId,
            int? categoryId,
            EventStatus? status);

        // Add other Event-specific query methods here if needed.
    }
}