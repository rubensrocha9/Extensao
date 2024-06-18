namespace GestorPay.Models.DTOs
{
    public class AttachmentDTO
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int? CompanyId { get; set; }
        public int? EmployeeId { get; set; }
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ImgUrl { get; set; }
    }
}
