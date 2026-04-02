using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Model.DTO.Categories;

namespace CASH.FLOW.TRACKER.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category category, CancellationToken ct);
        Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken ct);
        Task<IEnumerable<GetCategoryDTO>> GetCategories(CancellationToken ct);
        Task<GetCategoryDTO?> GetCategoryByIdAsync(int categoryId, CancellationToken ct);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken ct);
    }
}
