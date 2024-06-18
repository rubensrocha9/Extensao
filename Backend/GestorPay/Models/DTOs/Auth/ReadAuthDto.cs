using GestorPay.Enumerator;

namespace GestorPay.Models.DTOs.Auth
{
    public class ReadAuthDto
    {
        public int? CompanyId { get; set; }
        public int? EmployeeId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public UserRoleType Role { get; set; }
    }
}
