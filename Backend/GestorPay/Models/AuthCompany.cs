using GestorPay.Helper;

namespace GestorPay.Models
{
    public class AuthCompany : BaseEntity
    {
        public string? Password { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? EmailToken { get; set; }
        public DateTime ResetPasswordExpiry { get; set; }
        public bool RegistrationLinkValidator { get; set; }

        public virtual Company Company { get; set; }
    }
}
