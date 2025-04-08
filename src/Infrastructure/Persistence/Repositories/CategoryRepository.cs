using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation specific to the Category entity.
    /// </summary>
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(EventManagementDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets a category by its name asynchronously. Assumes names are unique.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The category if found; otherwise, null.</returns>
        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            // Use case-insensitive comparison for robustness, depending on requirements
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
                // Or use EF.Functions.Like for case-insensitive search depending on DB collation
                // return await _dbSet.FirstOrDefaultAsync(c => EF.Functions.Like(c.Name, name));
        }
    }
}