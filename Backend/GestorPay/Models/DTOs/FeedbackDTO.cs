namespace GestorPay.Models.DTOs
{
    public class FeedbackDTO
    {
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string Feedback { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
