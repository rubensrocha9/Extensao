namespace GestorPay.Models.DTOs.User
{
    public class CreateUserDto
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string DocumentNumber { get; set; }
        public string Password { get; set; }
    }
}
