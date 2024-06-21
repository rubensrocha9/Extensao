using GestorPay.Context;
using GestorPay.Models.DTOs;
using GestorPay.Models.DTOs.Auth;
using GestorPay.Models.DTOs.User;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestorPay.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GestorPayContext _context;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;

        public AuthController(GestorPayContext context, IAuthService authService, INotificationService notificationService)
        {
            _context = context;
            _authService = authService;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("api/authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] CreateAuthDto userObj)
        {
            return Ok(await _authService.Authenticate(userObj));
        }

        [HttpPost]
        [Route("api/company/register")]
        public async Task<IActionResult> RegisterCompanyUser([FromBody] CreateUserDto userObj)
        {
            await _authService.Register(userObj);
            return Ok();
        }

        [HttpPost]
        [Route("api/company/{companyId}/employee/{id}/register")]
        public async Task<IActionResult> RegisterEmployeeUser(int companyId, int id, [FromBody] CreateEmployeeUserDTO userObj)
        {
            await _authService.RegisterEmployee(companyId, id, userObj);
            return Ok();
        }

        [HttpPost]
        [Route("api/confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmail)
        {
            await _authService.ConfirmEmail(confirmEmail);
            return Ok();
        }

        [HttpPost]
        [Route("api/company/employee/confirm-email")]
        public async Task<IActionResult> ConfirmEmployeeEmail([FromBody] ConfirmEmailDto confirmEmail)
        {
            await _authService.ConfirmEmployeeEmail(confirmEmail);
            return Ok();
        }

        [HttpPost("api/send/reset-password")]
        public async Task<IActionResult> SendEmailResetPassword(ResetPasswordDTO email)
        {
            await _authService.SendEmailResetPassword(email);
            return Ok();
        }

        [HttpPost("api/reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
            await _authService.ResetPassword(resetPassword);
            return Ok();
        }
    }
}
