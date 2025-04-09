using EventManagement.Application.DTOs.Category;
using EventManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime; // For ProducesResponseType
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)] // Specify default content type
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of categories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(CancellationToken cancellationToken)
        {
            _logger.LogInformation("API call: Getting all categories.");
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            return Ok(categories);
        }

        /// <summary>
        /// Gets a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested category.</returns>
        [HttpGet("{id:int}", Name = "GetCategoryById")] // Added route name for CreatedAtRoute
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("API call: Getting category with ID: {CategoryId}", id);
            var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("API call: Category with ID: {CategoryId} not found.", id);
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="createCategoryDto">The category data to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created category.</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("API call: Invalid model state for creating category.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("API call: Creating new category with name: {CategoryName}", createCategoryDto.Name);
            var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDto, cancellationToken);

            // Return 201 Created with a Location header pointing to the new resource
            return CreatedAtRoute("GetCategoryById", new { id = createdCategory.Id }, createdCategory);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="updateCategoryDto">The updated category data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id:int}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken)
        {
            if (id != updateCategoryDto.Id)
            {
                 _logger.LogWarning("API call: Mismatched ID in route ({RouteId}) and body ({BodyId}) for category update.", id, updateCategoryDto.Id);
                return BadRequest("ID mismatch between route parameter and request body.");
            }

            if (!ModelState.IsValid)
            {
                 _logger.LogWarning("API call: Invalid model state for updating category ID: {CategoryId}.", id);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("API call: Updating category with ID: {CategoryId}", id);
            var success = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto, cancellationToken);

            if (!success)
            {
                // Could be NotFound or potentially another issue, service layer handles logging NotFound
                 _logger.LogWarning("API call: Update failed for category ID: {CategoryId}. It might not exist.", id);
                return NotFound(); // Assuming failure means not found, adjust if service provides more detail
            }

            return NoContent(); // Standard response for successful PUT
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("API call: Deleting category with ID: {CategoryId}", id);
            var success = await _categoryService.DeleteCategoryAsync(id, cancellationToken);

            if (!success)
            {
                 _logger.LogWarning("API call: Delete failed for category ID: {CategoryId}. It might not exist.", id);
                return NotFound(); // Standard response if the resource to delete is not found
            }

            return NoContent(); // Standard response for successful DELETE
        }
    }
}