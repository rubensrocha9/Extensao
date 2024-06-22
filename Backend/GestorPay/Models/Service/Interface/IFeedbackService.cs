using GestorPay.Models.DTOs;

namespace GestorPay.Models.Service.Interface
{
    public interface IFeedbackService
    {
        Task<FeedbackWithAttachmentDTO> GetEmployeeAllFeedbackAsync(int companyId);
        Task<List<FeedbackDTO>> GetFeedbackHistory(int companyId, int id);
        Task<AttachmentDTO> GetEmployeeAttachmentAsync(int id);
        Task CreateEmployeeFeedbackAsync(int companyId, int id, FeedbackDTO feedback);
        Task<AttachmentDTO> EmployeeAttachmentAsync(int id, IFormFile image);
    }
}
