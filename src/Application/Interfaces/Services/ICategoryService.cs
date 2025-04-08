using EventManagement.Application.DTOs.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for managing Category entities.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A collection of Category DTOs.</returns>
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The Category DTO or null if not found.</returns>
        Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="createCategoryDto">The DTO containing category creation data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created Category DTO.</returns>
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="updateCategoryDto">The DTO containing category update data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if update was successful, false otherwise.</returns>
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deletion was successful, false otherwise.</returns>
        Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
    }
}