using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Model.DTO;
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
                throw new InvalidOperationException("Failed to add new category");

            return true;
        } 
    }
}
