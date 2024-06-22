using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IFeedbackService _feedbackService;

        public DashboardController(IDashboardService dashboardService, IFeedbackService feedbackService)
        {
            _dashboardService = dashboardService;
            _feedbackService = feedbackService;

        }

        [HttpGet]
        [Route("api/company/{companyId}/dashboard")]
        public async Task<IActionResult> GetEmployeeDashboard(int companyId)
        {
            return Ok(await _dashboardService.GetEmployeeDashboardAsync(companyId));
        }

        [HttpGet]
        [Route("api/company/{companyId}/feedback")]
        public async Task<IActionResult> GetEmployeeFeedback(int companyId)
        {
            return Ok(await _feedbackService.GetEmployeeAllFeedbackAsync(companyId));
        }
    }
}
