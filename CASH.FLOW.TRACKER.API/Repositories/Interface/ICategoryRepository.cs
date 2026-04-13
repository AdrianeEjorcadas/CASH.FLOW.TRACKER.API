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
        Task<IEnumerable<GetCategoryDTO>> GetCategories(Guid userId, CancellationToken ct);
        Task<PagedList<GetCategoryDTO>> GetCategoriesAsync(CategoryParameters categoryParameters, CancellationToken ct);
        Task<GetCategoryDTO?> GetCategoryByIdAsync(GetCategoryByIdDTO categoryByIdDTO, CancellationToken ct);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken ct);
    }
}
