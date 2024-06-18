using GestorPay.Enumerator;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;

namespace GestorPay.Models.Service.Interface
{
    public interface IEmployeePermissionService
    {
        Task<PageList<EmployeePermissionDTO>> GetEmployeesPermissionsAsync(PageParams pageParams, int companyId);
        Task<EmployeePermissionDTO> GetEmployeesPermissionsByIdAsync(int companyId, int employeeId);
        Task UpdateEmployeePermissionAsync(int companyId, int Id, UserRoleType userRole);
    }
}
