using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.Service.Interface;

namespace GestorPay.Models
{
    public class Company : BaseEntity, IAuthenticatable
    {
        public int AuthId { get; set; }
        public string Name { get; set; }
        public string DocumentNumber { get; set; }
        public string? Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public UserRoleType Role { get; set; }
        public bool IsCompany { get; set; }
        public bool IsRemoved { get; set; }

        public virtual AuthCompany Auth { get; set; }
        public virtual CompanyAddress Address { get; set; }
        public virtual Attachment Attachments { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<EmployeePosition> EmployeePositions { get; set; }
        public virtual ICollection<SpendingManager> SpendingManagers { get; set; }
    }
}
