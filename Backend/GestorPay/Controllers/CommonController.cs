using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        [Route("api/common/enums/{name}")]
        public async Task<IActionResult> GetEnumValuesAsync(string name)
        {
            return Ok(await _commonService.GetEnumValuesAsync(name.GetEnumTypeByName()));
        }
    }
}
