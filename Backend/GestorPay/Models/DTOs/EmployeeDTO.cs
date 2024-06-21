using GestorPay.Enumerator;
using GestorPay.Models.Helper;

namespace GestorPay.Models.DTOs
{
    public class EmployeeDTO
    {
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DocumentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public GenderStatusType Gender { get; set; }
        public string GenderDescription => Gender.GetDescription();
        public string? CompanyTime { get; set; }
        public EmployeeStatusType? Status { get; set; }
        public string StatusDescription => Status.GetDescription();
        public UserRoleType? Role { get; set; }
        public string RoleDescription => Role.GetDescription();
        public DateTime? CreationDate { get; set; }

        public EmployeeAddressDTO Address { get; set; }
        public AttachmentDTO Attachment { get; set; }
        public ICollection<EmployeeFeedback> Feedbacks { get; set; }
    }
}
