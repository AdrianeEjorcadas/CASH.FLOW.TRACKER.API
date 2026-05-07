using CASH.FLOW.TRACKER.API.Model.DTO.Dashboard;
using CASH.FLOW.TRACKER.API.Model.Response;
using CASH.FLOW.TRACKER.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CASH.FLOW.TRACKER.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("dashboard-initial-data")]
        public async Task<ActionResult<ReturnResponse<IEnumerable<InitialDataDto>>>> InitialDataAsync([FromQuery]GetInitialDataDto dto, CancellationToken ct = default)
        {
            var payload = await _dashboardService.GetInitialData(dto, ct);

            return Ok(new ReturnResponse<IEnumerable<InitialDataDto>>
            {
                StatusCode = 200,
                Data = payload,
                Message = "Dashboard initial data loaded succesfully"
            });
        }
    }
}
