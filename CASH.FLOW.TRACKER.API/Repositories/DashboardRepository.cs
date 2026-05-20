using BUGET.TRACKER.API.Data;
using BUGET.TRACKER.API.Model;
using CASH.FLOW.TRACKER.API.Model.DTO.Dashboard;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CASH.FLOW.TRACKER.API.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InitialDataDto>> GetInitialData(GetInitialDataDto dto, CancellationToken ct)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var payload = await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .Where(t => t.UserId == dto.userId
                    && t.TransactionDate >= dto.startDate
                    && t.TransactionDate <= dto.endDate)
                .GroupBy(t => t.Category.CategoryType)
                .Select(g => new InitialDataDto 
                {
                    CategoryType = g.Key,
                    Amount = g.Sum(x => x.Amount)
                })
                .ToListAsync(ct);

            return payload;
        }

        public async Task<IEnumerable<MonthlyBreakdownDto>> GetMonthlyBreakdown(Guid userId, CancellationToken ct)
        {
            var now = DateTimeOffset.UtcNow;
            var startDate = now.AddMonths(-6);
            var endDate = now;

            var monthlyBreakdown = await _context.Transactions
                .Where(t => !t.DeletedAt.HasValue &&
                            t.UserId == userId &&
                            t.TransactionDate >= startDate &&
                            t.TransactionDate < endDate)
                .Include(t => t.Category)
                .GroupBy(t => new
                {
                    Year = t.TransactionDate.Year,
                    Month = t.TransactionDate.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Categories = g.GroupBy(x => x.Category.CategoryType)
                                  .Select(cg => new CategoryBreakdownDto
                                  {
                                      Category = cg.Key,
                                      Amount = cg.Sum(x => x.Amount)
                                  })
                                  .ToList()
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ToListAsync();

            // Convert to DTO with DateTimeOffset after query materializes
            var result = monthlyBreakdown.Select(g => new MonthlyBreakdownDto
            {
                Month = new DateTimeOffset(g.Year, g.Month, 1, 0, 0, 0, TimeSpan.Zero),
                Categories = g.Categories
            }).ToList();


            return result;
        }
    }
}
