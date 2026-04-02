using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Middleware.Exceptions;
using CASH.FLOW.TRACKER.API.Model.DTO.Categories;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using CASH.FLOW.TRACKER.API.Services.Interface;

namespace CASH.FLOW.TRACKER.API.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<bool> AddCategoryAsync(AddCategoryDTO categoryDTO, CancellationToken ct)
        {
            var category = new Category
            {
                CategoryName = categoryDTO.CategoryName,
                CategoryType = categoryDTO.CategoryType,
            };

            var payload = await _categoryRepo.AddCategoryAsync(category, ct);

            if (payload is null)
                throw new CategoryException(categoryDTO.CategoryName);

            return true;
        }

        public async Task<GetCategoryDTO> GetCategoryByIdAsync(int categoryId, CancellationToken ct)
        {
            var payload = await _categoryRepo.GetCategoryByIdAsync(categoryId, ct);

            if (payload is null)
                throw new CategoryNotFoundException(categoryId);

            return payload;
        }

        public async Task<IEnumerable<GetCategoryDTO>> GetCategories(CancellationToken ct)
        {
            var payload = await _categoryRepo.GetCategories(ct);

            if (!payload.Any())
                throw new NoCategoryExistingException();

            return payload;
        }

        public async Task DeleteCategoryAsync(int categoryId, CancellationToken ct)
        {
            var isExisting = await _categoryRepo.DeleteCategoryAsync(categoryId, ct);

            if (!isExisting)
                throw new CategoryNotFoundException(categoryId);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken ct)
        {
            var isExisting = await _categoryRepo.UpdateCategoryAsync(updateCategoryDTO, ct);

            if (!isExisting)
                throw new CategoryNotFoundException(updateCategoryDTO.CategoryId);
        }

    }
}
