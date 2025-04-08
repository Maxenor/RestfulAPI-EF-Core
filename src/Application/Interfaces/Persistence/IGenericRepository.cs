using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A read-only list of all entities.</returns>
        Task<IReadOnlyList<T>> ListAllAsync();

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity (potentially with updated state like generated ID).</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// EF Core tracks changes, so this might just involve saving changes after modifying the entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        Task DeleteAsync(T entity);

        // Consider adding methods like:
        // Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        // Task<int> CountAsync(ISpecification<T> spec);
        // Task<T?> FirstOrDefaultAsync(ISpecification<T> spec);
    }
}