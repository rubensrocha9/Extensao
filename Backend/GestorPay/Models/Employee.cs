using GestorPay.Enumerator;
using GestorPay.Helper;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorPay.Models
{
    public class Employee : BaseEntity
    {
        public int CompanyId { get; set; }
        public int PositionId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DocumentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }

        public GenderStatusType Gender { get; set; }
        [NotMapped]
        public string CompanyTime { get; set; }
        public EmployeeStatusType Status { get; set; }
        public UserRoleType Role { get; set; }
        public bool IsRemoved { get; set; }

        //relacionamento 
        public virtual Company Company { get; set; }
        public virtual AuthEmployee AuthEmployee { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual EmployeePosition EmployeePosition { get; set; }
        public virtual EmployeeAddress Address { get; set; }
        public virtual ICollection<EmployeeFeedback> EmployeeFeedback { get; set; }  
    }
}
