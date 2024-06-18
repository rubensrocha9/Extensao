using GestorPay.Helper;

namespace GestorPay.Models
{
    public class EmployeePosition : BaseEntity
    {
        public int CompanyId { get; set; }
        public string PositionName { get; set; }
        public bool IsRemoved { get; set; }

        public virtual Company Company { get; set; }
    }
}
