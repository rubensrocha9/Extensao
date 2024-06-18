using GestorPay.Context;
using GestorPay.Models.DTOs;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService, GestorPayContext context)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Route("api/company/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _companyService.GetByIdAsync(id));
        }

        [HttpPut]
        [Route("api/company/{id}/update")]
        public async Task<IActionResult> UpdateCompanyAsync(int id, [FromBody] CompanyDTO updateCompany)
        {
            await _companyService.UpdateCompanyAsync(id, updateCompany);
            return Ok();
        }

        [HttpGet]
        [Route("api/company/{id}/file")]
        public async Task<IActionResult> GetCompanyAttachmentAsync(int id)
        {
            return Ok(await _companyService.GetCompanyAttachmentAsync(id));
        }

        [HttpPut]
        [Route("api/company/{id}/file")]
        public async Task<IActionResult> CompanyAttachmentAsync(int id, [FromForm] IFormFile image)
        {
            return Ok(await _companyService.CompanyAttachmentAsync(id, image));
        }

        [HttpDelete]
        [Route("api/company/{id}/deactivate-account")]
        public async Task<IActionResult> DeleteCompanyAsync(int id)
        {
            await _companyService.DeleteCompanyAsync(id);
            return Ok();
        }
    }
}
