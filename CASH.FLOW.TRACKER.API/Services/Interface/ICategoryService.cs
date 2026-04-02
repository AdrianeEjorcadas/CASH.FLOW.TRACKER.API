using CASH.FLOW.TRACKER.API.Model.DTO.Categories;

namespace CASH.FLOW.TRACKER.API.Services.Interface
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryAsync(AddCategoryDTO categoryDTO, CancellationToken ct);
        Task DeleteCategoryAsync(int categoryId, CancellationToken ct);
        Task<IEnumerable<GetCategoryDTO>> GetCategories(CancellationToken ct);
        Task<GetCategoryDTO> GetCategoryByIdAsync(int categoryId, CancellationToken ct);
        Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken ct);
    }
}
