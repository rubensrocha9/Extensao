using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("api/company/{companyId}/dashboard")]
        public async Task<IActionResult> GetEmployeeDashboard(int companyId)
        {
            return Ok(await _dashboardService.GetEmployeeDashboardAsync(companyId));
        }
    }
}
