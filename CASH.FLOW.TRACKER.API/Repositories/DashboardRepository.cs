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
    }
}
