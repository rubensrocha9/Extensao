namespace GestorPay.Models.DTOs
{
    public class FeedbackDTO
    {
        
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string Feedback { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class FeedbackWithAttachmentDTO
    {
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public List<FeedbackDTO> Feedback { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
