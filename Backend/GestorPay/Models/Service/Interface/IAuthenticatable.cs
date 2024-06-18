using GestorPay.Enumerator;

namespace GestorPay.Models.Service.Interface
{
    public interface IAuthenticatable
    {
        int Id { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        bool IsEmailConfirmed { get; set; }
        UserRoleType Role { get; set; }
        AuthCompany Auth { get; set; }
    }
}
