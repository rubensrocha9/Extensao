using GestorPay.Helper;

namespace GestorPay.Models
{
    public class Attachment : BaseEntity
    {
        public int? CompanyId { get; set; }
        public int? EmployeeId { get; set; }
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool IsRemoved { get; set; }

        public virtual Company Company { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
