using GestorPay.Enumerator;
using GestorPay.Extentions;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Route("api/company/{companyId}/employee")]
        public async Task<IActionResult> GetEmployee([FromQuery] PageParams pageParams, int companyId)
        {
            var employees = await _employeeService.GetEmployeeAsync(pageParams, companyId);
            Response.AddPagination(employees.CurrentPage, employees.PageSize, employees.TotalCount, employees.TotalPages);

            return Ok(employees);
        }

        [HttpGet]
        [Route("api/company/{companyId}/employee/{id}")]
        public async Task<IActionResult> GetById(int companyId, int id)
        {
            return Ok(await _employeeService.GetByIdAsync(companyId, id));
        }

        [HttpPost]
        [Route("api/company/{companyId}/employee")]
        public async Task<IActionResult> CreateEmployee(int companyId, [FromBody] EmployeeAndAddressDTO createAddress)
        {
            await _employeeService.CreateEmployeeAsync(companyId, createAddress);
            return Ok();
        }

        [HttpPut]
        [Route("api/company/{companyId}/employee/{id}/update")]
        public async Task<IActionResult> PutEmployee(int companyId, int id, [FromBody] EmployeeAndAddressDTO updateEmployee)
        {
            await _employeeService.UpdateEmployeeAsync(companyId, id, updateEmployee, UserRoleType.Admin);
            return Ok();
        }

        [HttpDelete]
        [Route("api/company/{companyId}/employee/{id}/delete")]
        public async Task<IActionResult> DeleteEmployee(int companyId, int id)
        {
            await _employeeService.DeleteEmployeeAsync(companyId, id);
            return Ok();
        }
    }
}
