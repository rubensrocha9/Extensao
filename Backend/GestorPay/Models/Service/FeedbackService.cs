using GestorPay.Context;
using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.DTOs;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GestorPay.Models.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository _repository;
        private readonly INotificationService _notificationService;

        public FeedbackService(IRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<List<FeedbackDTO>> GetFeedbackHistory(int companyId, int employeeId)
        {
            var employee = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId &&
                            p.Id == employeeId &&
                            !p.IsRemoved)
                .AsNoTracking()
                .AnyAsync();

            if (!employee)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var feedbacks = await _repository.Select<EmployeeFeedback>()
                .Where(p => p.CompanyId == companyId &&
                            p.EmployeeId == employeeId &&
                            !p.IsRemoved)
                .Select(p => new FeedbackDTO
                {
                    CompanyId = p.CompanyId,
                    EmployeeId = p.EmployeeId,
                    Feedback = p.Feedback,
                    CreationDate = p.CreationDate
                })
                .AsNoTracking()
                .ToListAsync();

            return feedbacks;
        }

        public async Task CreateEmployeeFeedbackAsync(int companyId, int id, FeedbackDTO feedback)
        {
            var employee = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId &&
                            p.Id == id &&
                            !p.IsRemoved)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var newFeedback = new EmployeeFeedback();
            newFeedback.CompanyId = employee.CompanyId;
            newFeedback.EmployeeId = employee.Id;
            newFeedback.Feedback = feedback.Feedback;
            newFeedback.IsRemoved = false;

            _repository.Insert(newFeedback);
            await _repository.SaveAsync();
        }

        public async Task<AttachmentDTO> GetEmployeeAttachmentAsync(int id)
        {
            var attachment = await _repository.Select<Attachment>()
                .Where(p => p.EmployeeId == id && !p.IsRemoved)
                .Select(p => new
                {
                    p.Id,
                    p.FileName,
                    p.FileExtension,
                    p.ContentType,
                    p.Base64,
                    p.CompanyId
                })
                .FirstOrDefaultAsync();

            if (attachment == null)
            {
                return new AttachmentDTO();
            }

            return new AttachmentDTO
            {
                Id = attachment.Id,
                CompanyId = attachment.CompanyId,
                FileName = attachment.FileName,
                FileExtension = attachment.FileExtension,
                Base64 = attachment.Base64,
                ImgUrl = "data:" + attachment.ContentType + ";base64," + attachment.Base64,
            };
        }

        public async Task<AttachmentDTO> EmployeeAttachmentAsync(int id, IFormFile image)
        {
            var attachment = await _repository.Select<Attachment>()
                .Where(p => p.EmployeeId == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (attachment == null)
            {
                var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                var fileExtension = Path.GetExtension(image.FileName);
                var contentType = image.ContentType;

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(fileBytes);

                    var newAttachment = new Attachment
                    {
                        EmployeeId = id,
                        FileName = fileName,
                        FileExtension = fileExtension,
                        ContentType = contentType,
                        Base64 = base64String,
                        CreationDate = DateTime.Now,
                    };

                    _repository.Insert(newAttachment);
                    await _repository.SaveAsync();

                    return new AttachmentDTO
                    {
                        Id = newAttachment.Id,
                        EmployeeId = id,
                        FileName = fileName,
                        FileExtension = fileExtension,
                        Base64 = base64String,
                        ImgUrl = "data:" + contentType + ";base64," + base64String,
                    };
                }
            }
            else
            {
                var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                var fileExtension = Path.GetExtension(image.FileName);
                var contentType = image.ContentType;

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var base64String = Convert.ToBase64String(fileBytes);

                    attachment.EmployeeId = id;
                    attachment.FileName = fileName;
                    attachment.FileExtension = fileExtension;
                    attachment.ContentType = contentType;
                    attachment.Base64 = base64String;

                    await _repository.SaveAsync();

                    return new AttachmentDTO
                    {
                        Id = attachment.Id,
                        EmployeeId = id,
                        FileName = fileName,
                        FileExtension = fileExtension,
                        Base64 = base64String,
                        ImgUrl = "data:" + contentType + ";base64," + base64String,
                    };
                }
            }
        }
    }
}
