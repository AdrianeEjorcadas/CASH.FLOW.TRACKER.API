using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Helpers.Pagination;
using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Model.DTO.Categories;

namespace CASH.FLOW.TRACKER.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category category, CancellationToken ct);
        Task<bool> DeleteCategoryAsync(DeleteCategoryDTO deleteCategoryDTO, CancellationToken ct);
        Task<IEnumerable<GetCategoryDTO>> GetCategories(CancellationToken ct);
        Task<PagedList<GetCategoryDTO>> GetCategoriesAsync(CategoryParameters categoryParameters, CancellationToken ct);
        Task<GetCategoryDTO?> GetCategoryByIdAsync(int categoryId, CancellationToken ct);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken ct);
    }
}
