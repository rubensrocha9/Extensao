using GestorPay.Models.DTOs.Auth;
using GestorPay.Models.DTOs.User;
using GestorPay.Models.DTOs;

namespace GestorPay.Models.Service.Interface
{
    public interface IAuthService
    {
        Task<ReadAuthDto> Authenticate(CreateAuthDto createAuth);
        Task Register(CreateUserDto createUser);
        Task RegisterEmployee(int companyId, int id, CreateEmployeeUserDTO createUser);
        Task SendEmailResetPassword(ResetPasswordDTO email);
        Task ResetPassword(ResetPasswordDTO resetPassword);
        Task ConfirmEmail(ConfirmEmailDto confirmEmail);
        Task ConfirmEmployeeEmail(ConfirmEmailDto confirmEmail);
    }
}
