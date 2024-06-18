using GestorPay.Helper;

namespace GestorPay.Models
{
    public class AuthEmployee : BaseEntity
    {
        public int AuthEmployeeId { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? EmailToken { get; set; }
        public DateTime ResetPasswordExpiry { get; set; }
        public bool RegistrationLinkValidator { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
