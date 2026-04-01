using CASH.FLOW.TRACKER.API.Model.DTO;

namespace CASH.FLOW.TRACKER.API.Services.Interface
{
    public interface ICategoryService
    {
        Task<bool> AddCategoryAsync(AddCategoryDTO categoryDTO, CancellationToken ct);
    }
}
