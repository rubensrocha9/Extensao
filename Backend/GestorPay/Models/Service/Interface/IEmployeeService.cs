using GestorPay.Enumerator;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;

namespace GestorPay.Models.Service.Interface
{
    public interface IEmployeeService
    {
        Task<PageList<EmployeeDTO>> GetEmployeeAsync(PageParams pageParams, int companyId);
        Task<EmployeeDTO> GetByIdAsync(int companyId, int id);
        Task CreateEmployeeAsync(int companyId, EmployeeAndAddressDTO createAddress);
        Task UpdateEmployeeAsync(int companyId, int id, EmployeeAndAddressDTO updateEmployee, UserRoleType userRole);
        Task DeleteEmployeeAsync(int companyId, int id);
    }
}
