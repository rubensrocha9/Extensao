using GestorPay.Helper;

namespace GestorPay.Models
{
    public class CompanyAddress : BaseEntity
    {
        public int CompanyId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? Complement { get; set; }
        public string? District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public bool IsRemoved { get; set; }

        public virtual Company Company { get; set; }
    }
}
