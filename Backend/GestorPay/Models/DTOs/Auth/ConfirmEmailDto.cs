namespace GestorPay.Models.DTOs.Auth
{
    public class ConfirmEmailDto
    {
        public int CompanyId { get; set; }
        public int? EmployeeId { get; set; }
        public string EmailToken { get; set; }
    }
}
