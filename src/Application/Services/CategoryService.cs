using AutoMapper;
using EventManagement.Application.DTOs.Category;
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all categories.");
            var categories = await _categoryRepository.ListAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving category with ID: {CategoryId}", id);
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with ID: {CategoryId} not found.", id);
                return null;
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new category with name: {CategoryName}", createCategoryDto.Name);
            var category = _mapper.Map<Category>(createCategoryDto);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var createdCategory = await _categoryRepository.AddAsync(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<CategoryDto>(createdCategory);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default)
        {
            if (id != updateCategoryDto.Id)
            {
                _logger.LogWarning("Mismatched ID in route ({RouteId}) and body ({BodyId}) during category update.", id, updateCategoryDto.Id);
                return false;
            }

            _logger.LogInformation("Updating category with ID: {CategoryId}", id);
            
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    _logger.LogWarning("Category with ID: {CategoryId} not found for update.", id);
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                _mapper.Map(updateCategoryDto, existingCategory);
                await _categoryRepository.UpdateAsync(existingCategory);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to delete category with ID: {CategoryId}", id);
            
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
                if (categoryToDelete == null)
                {
                    _logger.LogWarning("Category with ID: {CategoryId} not found for deletion.", id);
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                await _categoryRepository.DeleteAsync(categoryToDelete);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();
                
                _logger.LogInformation("Successfully deleted category with ID: {CategoryId}", id);
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}