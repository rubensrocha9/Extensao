using GestorPay.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorPay.Models.Helper
{
    public class HelperValidation
    {
        private readonly GestorPayContext _context;

        public HelperValidation(GestorPayContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            var existCompanyEmail = await _context.Set<Company>().Where(p => !p.IsRemoved).AnyAsync(x => x.Email == email);
            var existEmployeeEmail = await _context.Set<Employee>().Where(p => !p.IsRemoved).AnyAsync(x => x.Email == email);

            return existCompanyEmail || existEmployeeEmail;
        }

        public async Task<bool> CheckDocumentExist(string documentNumber)
        {
            var existCompanyEmail = await _context.Set<Company>().Where(p => !p.IsRemoved).AnyAsync(x => x.DocumentNumber == documentNumber);
            var existEmployeeEmail = await _context.Set<Employee>().Where(p => !p.IsRemoved).AnyAsync(x => x.DocumentNumber == documentNumber);

            return existCompanyEmail || existEmployeeEmail;
        }

        public async Task<bool> IsPasswordStrong(string password)
        {
            if (password.Length < 8)
                return false;

            if (!password.Any(char.IsUpper))
                return false;

            if (!password.Any(char.IsLower))
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return false;

            return true;
        }
    }
}
