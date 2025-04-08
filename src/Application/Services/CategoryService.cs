using AutoMapper;
using EventManagement.Application.DTOs.Category;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;
using Microsoft.Extensions.Logging; // Added for potential logging
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger; // Optional: for logging

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Optional
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default) // Keep CancellationToken here for potential future use or consistency at service layer
        {
            _logger.LogInformation("Retrieving all categories.");
            // Use ListAllAsync and ignore cancellationToken for repository call based on IGenericRepository
            var categories = await _categoryRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default) // Keep CancellationToken
        {
            _logger.LogInformation("Retrieving category with ID: {CategoryId}", id);
            // Use GetByIdAsync without cancellationToken
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID: {CategoryId} not found.", id);
                return null;
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default) // Keep CancellationToken
        {
            _logger.LogInformation("Creating a new category with name: {CategoryName}", createCategoryDto.Name);
            var category = _mapper.Map<Category>(createCategoryDto);
            // Use AddAsync without cancellationToken
            var createdCategory = await _categoryRepository.AddAsync(category);
            // Assuming AddAsync returns the added entity with its ID populated
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default)
        {
            if (id != updateCategoryDto.Id)
            {
                 _logger.LogWarning("Mismatched ID in route ({RouteId}) and body ({BodyId}) during category update.", id, updateCategoryDto.Id);
                // Consider throwing an exception or returning a specific result for bad requests
                return false; // Or throw new ArgumentException("ID mismatch");
            }

            _logger.LogInformation("Updating category with ID: {CategoryId}", id);
            // Use GetByIdAsync without cancellationToken
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            if (existingCategory == null)
            {
                _logger.LogWarning("Category with ID: {CategoryId} not found for update.", id);
                return false;
            }

            // Map updated fields onto the existing entity
            _mapper.Map(updateCategoryDto, existingCategory);

            // Use UpdateAsync without cancellationToken
            await _categoryRepository.UpdateAsync(existingCategory);
            return true; // UpdateAsync typically doesn't return a value, success is assumed if no exception
        }

        public async Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default) // Keep CancellationToken
        {
            _logger.LogInformation("Attempting to delete category with ID: {CategoryId}", id);
            // Use GetByIdAsync without cancellationToken
            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);

            if (categoryToDelete == null)
            {
                _logger.LogWarning("Category with ID: {CategoryId} not found for deletion.", id);
                return false;
            }

            // Use DeleteAsync without cancellationToken
            await _categoryRepository.DeleteAsync(categoryToDelete);
             _logger.LogInformation("Successfully deleted category with ID: {CategoryId}", id);
            return true; // DeleteAsync typically doesn't return a value, success is assumed if no exception
        }
    }
}