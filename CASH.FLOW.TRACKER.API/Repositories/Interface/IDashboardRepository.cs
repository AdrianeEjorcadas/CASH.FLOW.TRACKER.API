using CASH.FLOW.TRACKER.API.Model.DTO.Dashboard;

namespace CASH.FLOW.TRACKER.API.Repositories.Interface
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<InitialDataDto>> GetInitialData(GetInitialDataDto dto, CancellationToken ct);
    }
}
