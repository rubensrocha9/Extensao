using GestorPay.Enumerator;
using GestorPay.Helper;

namespace GestorPay.Models
{
    public class SpendingManager : BaseEntity
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public SpendingManagerStatusType Status { get; set; }
        public bool IsExpenseEmployee { get; set; }
        public bool IsRemoved { get; set; }

        public virtual Company Company { get; set; }
    }
}
