using EventManagement.Application.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Generic repository implementation providing common CRUD operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly EventManagementDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(EventManagementDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            // FindAsync is optimized for finding by primary key
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync(); // Use AsNoTracking for read-only queries
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            // Note: SaveChangesAsync is typically called by a Unit of Work pattern
            // or at the end of a service method, not within each repository method.
            // We'll handle saving later.
            return entity;
        }

        public virtual Task UpdateAsync(T entity)
        {
            // EF Core tracks changes automatically when an entity is retrieved and modified.
            // Explicitly setting the state can be useful if the entity is detached.
            _dbContext.Entry(entity).State = EntityState.Modified;
            // Again, SaveChangesAsync is handled elsewhere.
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            // SaveChangesAsync handled elsewhere.
            return Task.CompletedTask;
        }

        // Potential place to implement methods using Specifications if that pattern is adopted later.
        // protected IQueryable<T> ApplySpecification(ISpecification<T> spec)
        // {
        //     return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
        // }
        //
        // public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        // {
        //     return await ApplySpecification(spec).AsNoTracking().ToListAsync();
        // }
    }
}