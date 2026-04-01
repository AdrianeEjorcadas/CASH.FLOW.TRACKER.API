using BUGET.TRACKER.API.Model;

namespace CASH.FLOW.TRACKER.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category category, CancellationToken ct);
    }
}
