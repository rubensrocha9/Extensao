using GestorPay.Extentions;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    [ApiController]
    public class EmployeePositionController : ControllerBase
    {
        private readonly IEmployeePositionService _positionService;

        public EmployeePositionController(IEmployeePositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        [Route("api/company/{companyId}/employee-position")]
        public async Task<IActionResult> GetEmployeePosition(int companyId, [FromQuery] PageParams pageParams)
        {
            var position = await _positionService.GetEmployeePositionsAsync(companyId, pageParams);
            Response.AddPagination(position.CurrentPage, position.PageSize, position.TotalCount, position.TotalPages);

            return Ok(position);
        }

        [HttpGet]
        [Route("api/company/{companyId}/list/employee-position")]
        public async Task<IActionResult> GetEmployeePositionAsync(int companyId)
        {
            return Ok(await _positionService.GetListPositionsAsync(companyId));
        }

        [HttpGet]
        [Route("api/company/{companyId}/employee-position/{id}")]
        public async Task<IActionResult> GetById(int companyId, int id)
        {
            return Ok(await _positionService.GetEmployeePositionByIdAsync(companyId, id));
        }

        [HttpPost]
        [Route("api/company/{companyId}/employee-position")]
        public async Task<IActionResult> PostEmployeePosition(int companyId, [FromBody] EmployeePositionDTO createPosition)
        {
            await _positionService.CreateEmployeePositionAsync(companyId, createPosition);
            return Ok();
        }

        [HttpPut]
        [Route("api/company/{companyId}/employee-position/{id}/update")]
        public async Task<IActionResult> PutEmployeePosition(int companyId, int id, [FromBody] EmployeePositionDTO updatePosition)
        {
            await _positionService.UpdateEmployeePositionAsync(companyId, id, updatePosition);
            return Ok();
        }

        [HttpDelete]
        [Route("api/company/{companyId}/employee-position/{id}/delete")]
        public async Task<IActionResult> DeleteCargo(int companyId, int id)
        {
            await _positionService.DeleteEmployeePositionAsync(companyId, id);
            return Ok();
        }

    }
}
