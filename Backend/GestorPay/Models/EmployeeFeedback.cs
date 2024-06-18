using GestorPay.Helper;

namespace GestorPay.Models
{
    public class EmployeeFeedback : BaseEntity
    {
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string Feedback { get; set; }
        public bool IsRemoved { get; set; }

        public virtual Company Company { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
