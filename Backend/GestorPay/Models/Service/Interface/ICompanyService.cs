using GestorPay.Models.DTOs;

namespace GestorPay.Models.Service.Interface
{
    public interface ICompanyService
    {
        Task<CompanyDTO> GetByIdAsync(int id);
        Task UpdateCompanyAsync(int id, CompanyDTO updateCompany);
        Task<AttachmentDTO> GetCompanyAttachmentAsync(int id);
        Task<AttachmentDTO> CompanyAttachmentAsync(int id, IFormFile Image);
        Task DeleteCompanyAsync(int id);
    }
}
