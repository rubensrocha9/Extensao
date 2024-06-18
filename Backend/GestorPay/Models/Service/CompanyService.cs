using GestorPay.Context;
using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.DTOs;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GestorPay.Models.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository _repository;
        private readonly INotificationService _notificationService;

        public CompanyService(IRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<CompanyDTO> GetByIdAsync(int id)
        {
            var company = await _repository.Select<Company>()
                .Where(p => p.Id == id && !p.IsRemoved)
                .Select(p => new CompanyDTO
                {
                    Name = p.Name,
                    DocumentNumber = p.DocumentNumber,
                    Email = p.Email,
                    IsEmailConfirmed = p.IsEmailConfirmed,
                    Role = p.Role,
                    CreationDate = p.CreationDate,
                    Address = new CompanyAddressDTO
                    {
                        Street = p.Address.Street ?? "",
                        City = p.Address.City ?? "",
                        Complement = p.Address.Complement ?? "",
                        District = p.Address.District ?? "",
                        Country = p.Address.Country ?? "",
                        Number = p.Address.Number ?? "",
                        State = p.Address.State ?? "",
                        ZipCode = p.Address.ZipCode ?? ""
                    }
                })
                .FirstOrDefaultAsync();

            if (company == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            return company;
        }

        public async Task UpdateCompanyAsync(int id, CompanyDTO updateCompany)
        {
            var company = await _repository.Select<Company>()
                .Where(p => p.Id == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            company.Name = updateCompany.Name;
            company.DocumentNumber = updateCompany.DocumentNumber;
            company.Email = updateCompany.Email;

            if (company.Address != null)
            {
                company.Address.ZipCode = updateCompany.Address.ZipCode;
                company.Address.Number = updateCompany.Address.Number;
                company.Address.Country = updateCompany.Address.Country;
                company.Address.City = updateCompany.Address.City;
                company.Address.Complement = updateCompany.Address.Complement;
                company.Address.District = updateCompany.Address.District;
                company.Address.State = updateCompany.Address.State;
                company.Address.Street = updateCompany.Address.Street;
            }
            else
            {
                company.Address = new CompanyAddress
                {
                    ZipCode = updateCompany.Address.ZipCode,
                    Number = updateCompany.Address.Number,
                    Country = updateCompany.Address.Country,
                    City = updateCompany.Address.City,
                    Complement = updateCompany.Address.Complement,
                    District = updateCompany.Address.District,
                    State = updateCompany.Address.State,
                    Street = updateCompany.Address.Street,
                };
            }

            await _repository.SaveAsync();
        }

        public async Task<AttachmentDTO> CompanyAttachmentAsync(int id, IFormFile image)
        {
            var attachment = await _repository.Select<Attachment>()
                .Where(p => p.CompanyId == id && !p.IsRemoved)
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
                        CompanyId = id,
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
                        CompanyId = id,
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

                    attachment.CompanyId = id;
                    attachment.FileName = fileName;
                    attachment.FileExtension = fileExtension;
                    attachment.ContentType = contentType;
                    attachment.Base64 = base64String;
                    attachment.LastUpdateDate = DateTime.Now;

                    await _repository.SaveAsync();

                    return new AttachmentDTO
                    {
                        Id = attachment.Id,
                        CompanyId = id,
                        FileName = fileName,
                        FileExtension = fileExtension,
                        Base64 = base64String,
                        ImgUrl = "data:" + contentType + ";base64," + base64String,
                    };
                }
            }
        }

        public async Task<AttachmentDTO> GetCompanyAttachmentAsync(int id)
        {
            var attachment = await _repository.Select<Attachment>()
                .Where(p => p.CompanyId == id && !p.IsRemoved)
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
                return new AttachmentDTO { };
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

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _repository.Select<Company>()
                .Include(p => p.Employees)
                .Include(p => p.EmployeePositions)
                .Include(p => p.SpendingManagers)
                .Where(p => p.Id == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            foreach (var manager in company.SpendingManagers.Where(p => !p.IsRemoved))
            {
                manager.IsRemoved = true;
            }

            foreach (var employee in company.Employees.Where(p => !p.IsRemoved))
            {
                employee.IsRemoved = true;

                if (employee.Attachment != null)
                {
                    employee.Attachment.IsRemoved = true;
                }
                else if (employee.Address != null)
                {
                    employee.Address.IsRemoved = true;
                }
                else if (employee.EmployeeFeedback != null)
                {
                    foreach (var feedback in employee.EmployeeFeedback)
                    {
                        feedback.IsRemoved = true;
                    }
                }
            }

            foreach (var position in company.EmployeePositions.Where(p => !p.IsRemoved))
            {
                position.IsRemoved = true;
            }

            company.Attachments.IsRemoved = true;
            company.Address.IsRemoved = true;
            company.IsRemoved = true;

            await _repository.SaveAsync();
        }
    }
}
