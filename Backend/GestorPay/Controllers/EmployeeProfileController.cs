using GestorPay.Enumerator;
using GestorPay.Models.DTOs;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    public class EmployeeProfileController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IFeedbackService _feedbackService;

        public EmployeeProfileController(IEmployeeService employeeService, IFeedbackService feedbackService)
        {
            _employeeService = employeeService;
            _feedbackService = feedbackService;
        }

        [HttpGet]
        [Route("api/company/{companyId}/employee/{id}/feedback")]
        public async Task<IActionResult> CreateEmployeeFeedbackAsync(int companyId, int id)
        {
            return Ok(await _feedbackService.GetFeedbackHistory(companyId, id));
        }

        [HttpGet]
        [Route("api/company/employee-profile/{id}/file")]
        public async Task<IActionResult> GetEmployeeAttachmentAsync(int id)
        {
            return Ok(await _feedbackService.GetEmployeeAttachmentAsync(id));
        }

        [HttpPost]
        [Route("api/company/{companyId}/employee/{id}/feedback")]
        public async Task<IActionResult> CreateEmployeeFeedbackAsync(int companyId, int id, [FromBody] FeedbackDTO feedback)
        {
            await _feedbackService.CreateEmployeeFeedbackAsync(companyId, id, feedback);
            return Ok();
        }

        [HttpPut]
        [Route("api/company/{companyId}/employee/{id}/update-profile")]
        public async Task<IActionResult> PutEmployee(int companyId, int id, [FromBody] EmployeeAndAddressDTO updateEmployee)
        {
            await _employeeService.UpdateEmployeeAsync(companyId, id, updateEmployee, UserRoleType.User);
            return Ok();
        }

        [HttpPut]
        [Route("api/company/employee-profile/{id}/file")]
        public async Task<IActionResult> EmployeeAttachmentAsync(int id, [FromForm] IFormFile image)
        {
            return Ok(await _feedbackService.EmployeeAttachmentAsync(id, image));
        }
    }
}
