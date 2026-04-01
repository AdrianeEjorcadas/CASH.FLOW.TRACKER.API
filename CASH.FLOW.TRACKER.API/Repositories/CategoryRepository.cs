using BUGET.TRACKER.API.Data;
using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Model.DTO;
using CASH.FLOW.TRACKER.API.Model.Response;
using CASH.FLOW.TRACKER.API.Repositories.Interface;

namespace CASH.FLOW.TRACKER.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategoryAsync(Category category, CancellationToken ct)
        {
            var payload = await _context.Categories
                .AddAsync(category, ct);

            await _context.SaveChangesAsync();

            return payload.Entity;
        }
    }
}
