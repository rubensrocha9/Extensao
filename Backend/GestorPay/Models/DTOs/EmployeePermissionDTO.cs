using GestorPay.Enumerator;
using GestorPay.Models.Helper;

namespace GestorPay.Models.DTOs
{
    public class EmployeePermissionDTO
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int PositionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRoleType Role { get; set; }
        public string RoleDescription => Role.GetDescription();
        public DateTime CreationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string EmployeePosition { get; set; }
        public string CompanyTime { get; set; }
        public string Address { get; set; }
    }
}
