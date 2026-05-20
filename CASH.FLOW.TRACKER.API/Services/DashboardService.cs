using CASH.FLOW.TRACKER.API.Middleware.Exceptions;
using CASH.FLOW.TRACKER.API.Model.DTO.Dashboard;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using CASH.FLOW.TRACKER.API.Services.Interface;

namespace CASH.FLOW.TRACKER.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IEnumerable<InitialDataDto>> GetInitialData(GetInitialDataDto dto, CancellationToken ct)
        {
            var payload = await _dashboardRepository.GetInitialData(dto, ct);

            if (!payload.Any())
                throw new TransactionNotFoundException();
            
            return payload;
        }

        public async Task<IEnumerable<MonthlyBreakdownDto>> GetMonthlyBreakdown(Guid userId, CancellationToken ct)
        {
            var payload = await _dashboardRepository.GetMonthlyBreakdown(userId, ct);

            if (!payload.Any())
                throw new TransactionNotFoundException();

            return payload;
        }
    }
}
