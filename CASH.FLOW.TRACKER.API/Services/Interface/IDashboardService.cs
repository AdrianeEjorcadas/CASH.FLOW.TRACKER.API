using CASH.FLOW.TRACKER.API.Model.DTO.Dashboard;

namespace CASH.FLOW.TRACKER.API.Services.Interface
{
    public interface IDashboardService
    {
        Task<IEnumerable<InitialDataDto>> GetInitialData(GetInitialDataDto dto, CancellationToken ct);
    }
}
