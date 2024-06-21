using GestorPay.Enumerator;
using GestorPay.Models.Helper;

namespace GestorPay.Models.DTOs
{
    public class CompanyDTO
    {
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public UserRoleType Role { get; set; }
        public string RoleDescription => Role.GetDescription();
        public DateTime CreationDate { get; set; }

        public CompanyAddressDTO Address { get; set; }
    }
}
