using GestorPay.Enumerator;
using GestorPay.Extentions;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    public class EmployeePermissionController : ControllerBase
    {
        private readonly IEmployeePermissionService _employeePermissionService;

        public EmployeePermissionController(IEmployeePermissionService employeePermissionService)
        {
            _employeePermissionService = employeePermissionService;
        }

        [HttpGet]
        [Route("api/company/{companyId}/employee/permission")]
        public async Task<IActionResult> GetEmployeePermission([FromQuery] PageParams pageParams, int companyId)
        {
            var permission = await _employeePermissionService.GetEmployeesPermissionsAsync(pageParams, companyId);
            Response.AddPagination(permission.CurrentPage, permission.PageSize, permission.TotalCount, permission.TotalPages);

            return Ok(permission);
        }

        [HttpGet]
        [Route("api/company/{companyId}/employee/{employeeId}/permission")]
        public async Task<IActionResult> GetEmployeePermissionById(int companyId, int employeeId)
        {
            return Ok(await _employeePermissionService.GetEmployeesPermissionsByIdAsync(companyId, employeeId));
        }

        [HttpPut]
        [Route("api/company/{companyId}/employee/{employeeId}/permission")]
        public async Task<IActionResult> UpdateEmployeePermissionAsync(int companyId, int employeeId, [FromBody] UserRoleType userRole)
        {
            await _employeePermissionService.UpdateEmployeePermissionAsync(companyId, employeeId, userRole);
            return Ok();
        }
    }
}
