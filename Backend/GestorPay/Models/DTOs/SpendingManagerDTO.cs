using GestorPay.Enumerator;
using GestorPay.Models.Helper;

namespace GestorPay.Models.DTOs
{
    public class SpendingManagerDTO
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public SpendingManagerStatusType Status { get; set; }
        public string StatusDescription => Status.GetDescription();
        public DateTime CreationDate { get; set; }
        public bool IsExpenseEmployee { get; set; }
    }
}
