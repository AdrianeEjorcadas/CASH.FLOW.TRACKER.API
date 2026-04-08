using BUGET.TRACKER.API.Data;
using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Helpers.Pagination;
using CASH.FLOW.TRACKER.API.Helpers.Pagination.Parameters;
using CASH.FLOW.TRACKER.API.Middleware.Exceptions;
using CASH.FLOW.TRACKER.API.Model.DTO;
using CASH.FLOW.TRACKER.API.Model.DTO.Categories;
using CASH.FLOW.TRACKER.API.Model.Response;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<GetCategoryDTO?> GetCategoryByIdAsync(int categoryId, CancellationToken ct)
        {
            var payload = await _context.Categories
                .AsNoTracking()
                .Select(c => new GetCategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryType = c.CategoryType,
                })
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            return payload;
        }

        public async Task<IEnumerable<GetCategoryDTO>> GetCategories(CancellationToken ct)
        {
            var payload = await _context.Categories
                .AsNoTracking()
                .Select(c => new GetCategoryDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryType = c.CategoryType,
                })
                .ToListAsync(ct);

            return payload;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId, CancellationToken ct)
        {
            var payload = await _context.Categories
                .FindAsync(new object?[] { categoryId }, ct);

            if(payload is null)
                return false;

            _context.Categories.Remove(payload);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO, CancellationToken ct)
        {
            var category = await _context.Categories
                .FindAsync(new object?[] { updateCategoryDTO.CategoryId }, ct);

            if (category is null)
                return false;

            if(updateCategoryDTO.CategoryName is not null)
                category.CategoryName = updateCategoryDTO.CategoryName;

            if (updateCategoryDTO.CategoryType is not null)
                category.CategoryType = updateCategoryDTO.CategoryType;

            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<PagedList<GetCategoryDTO>> GetCategoriesAsync(CategoryParameters categoryParameters, CancellationToken ct)
        {
            var query = _context.Categories.AsQueryable();
            var count = 0;

            //SEARCH TERM
            if (!string.IsNullOrEmpty(categoryParameters.SearchTerm))
            {
                query = query.Where(q => q.CategoryName.Contains(categoryParameters.SearchTerm));
            }

            count = await query.CountAsync();

            var result = await query
                .AsNoTracking()
                .OrderBy(q => q.CategoryName)
                .Skip((categoryParameters.PageNumber -1) * categoryParameters.PageSize)
                .Take(categoryParameters.PageSize)
                .Select(q => new GetCategoryDTO
                {
                    CategoryId = q.CategoryId,
                    CategoryName = q.CategoryName,
                    CategoryType = q.CategoryType,
                })
                .ToListAsync(ct);

            return PagedList<GetCategoryDTO>
                .ToPagedList(result, count, categoryParameters.PageNumber, categoryParameters.PageSize);
        }
    }
}
