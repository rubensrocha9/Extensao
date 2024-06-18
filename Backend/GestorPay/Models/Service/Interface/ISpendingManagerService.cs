using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;

namespace GestorPay.Models.Service.Interface
{
    public interface ISpendingManagerService
    {
        Task<PageList<SpendingManagerDTO>> GetSpendingManagerAsync(int companyId, PageParams pageParams);
        Task<SpendingManagerDTO> GetSpendingManagerByIdAsync(int id, int companyId);
        Task CreateSpendingManagerAsync(int companyId, SpendingManagerDTO createSpending);
        Task UpdateSpendingManagerAsync(int companyId, int id, SpendingManagerDTO updateSpending);
        Task DeleteSpendingManagerAsync(int companyId, int id);
    }
}
