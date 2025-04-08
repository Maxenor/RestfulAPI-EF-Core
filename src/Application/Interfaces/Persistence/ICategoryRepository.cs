using EventManagement.Domain.Entities;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Persistence
{
    /// <summary>
    /// Repository interface specific to the Category entity.
    /// Inherits common CRUD operations from IGenericRepository.
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        /// <summary>
        /// Gets a category by its name asynchronously. Assumes names are unique.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The category if found; otherwise, null.</returns>
        Task<Category?> GetCategoryByNameAsync(string name);

        // Add other Category-specific query methods here if needed.
    }
}