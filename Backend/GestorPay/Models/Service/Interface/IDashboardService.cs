using GestorPay.Models.DTOs;

namespace GestorPay.Models.Service.Interface
{
    public interface IDashboardService
    {
        Task<EmployeeDashboardDTO> GetEmployeeDashboardAsync(int companyId);
    }
}
