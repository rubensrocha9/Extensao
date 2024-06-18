using GestorPay.Extentions;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    public class SpendingManagerController : ControllerBase
    {
        private readonly ISpendingManagerService _spendingManager;
        public SpendingManagerController(ISpendingManagerService spendingManager)
        {
            _spendingManager = spendingManager;
        }

        [HttpGet]
        [Route("api/company/{companyId}/spending-manager")]
        public async Task<IActionResult> GetSpendingManagerAsync(int companyId, [FromQuery] PageParams pageParams)
        {
            var manager = await _spendingManager.GetSpendingManagerAsync(companyId, pageParams);
            Response.AddPagination(manager.CurrentPage, manager.PageSize, manager.TotalCount, manager.TotalPages);

            return Ok(manager);
        }

        [HttpGet]
        [Route("api/company/{companyId}/spending-manager/{id}")]
        public async Task<IActionResult> GetSpendingManagerByIdAsync(int companyId, int id)
        {
            return Ok(await _spendingManager.GetSpendingManagerByIdAsync(id, companyId));
        }

        [HttpPost]
        [Route("api/company/{companyId}/spending-manager")]
        public async Task<IActionResult> CreateSpendingManagerAsync(int companyId, [FromBody] SpendingManagerDTO createSpending)
        {
            await _spendingManager.CreateSpendingManagerAsync(companyId, createSpending);
            return Ok();
        }

        [HttpPut]
        [Route("api/company/{companyId}/spending-manager/{id}/update")]
        public async Task<IActionResult> UpdateSpendingManagerAsync(int companyId, int id, [FromBody] SpendingManagerDTO updateSpending)
        {
            await _spendingManager.UpdateSpendingManagerAsync(companyId, id, updateSpending);
            return Ok();
        }

        [HttpDelete]
        [Route("api/company/{companyId}/spending-manager/{id}/delete")]
        public async Task<IActionResult> DeleteSpendingManagerAsync(int companyId, int id)
        {
            await _spendingManager.DeleteSpendingManagerAsync(companyId, id);
            return Ok();
        }
    }
}
