using GestorPay.Enumerator;

namespace GestorPay.Models.DTOs
{
    public class CreateEmployeeDTO
    {
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public int PositionId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DocumentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public GenderStatusType Gender { get; set; }
        public EmployeeStatusType? Status { get; set; }
    }
}
