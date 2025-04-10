using EventManagement.Domain.Entities;

namespace EventManagement.Application.Interfaces.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetCategoryByNameAsync(string name);
    }
}