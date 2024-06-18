using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
namespace GestorPay.Models.Service.Interface
{
    public interface IEmployeePositionService
    {
        Task<PageList<EmployeePositionDTO>> GetEmployeePositionsAsync(int companyId, PageParams pageParams);
        Task<List<EmployeePositionDTO>> GetListPositionsAsync(int companyId);
        Task<EmployeePositionDTO> GetEmployeePositionByIdAsync(int companyId, int id);
        Task CreateEmployeePositionAsync(int companyId, EmployeePositionDTO employeePosition);
        Task UpdateEmployeePositionAsync(int companyId, int id, EmployeePositionDTO employeePosition);
        Task DeleteEmployeePositionAsync(int companyId, int id);
    }
}
