using CASH.FLOW.TRACKER.API.Helpers.Pagination;
using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Model.DTO.Categories;

namespace CASH.FLOW.TRACKER.API.Services.Interface
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryAsync(AddCategoryDTO categoryDTO, CancellationToken ct);
        Task DeleteCategoryAsync(DeleteCategoryDTO deleteCategoryDTO, CancellationToken ct);
        Task<IEnumerable<GetCategoryDTO>> GetCategories(Guid userId, CancellationToken ct);
        Task<(IEnumerable<GetCategoryDTO> category, Metadata metadata)> GetCategoriesAsync(CategoryParameters categoryParameters, CancellationToken ct);
        Task<GetCategoryDTO> GetCategoryByIdAsync(GetCategoryByIdDTO categoryByIdDTO, CancellationToken ct);
        Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken ct);
    }
}
