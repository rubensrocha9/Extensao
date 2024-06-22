using GestorPay.Context;
using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;

namespace GestorPay.Models.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmailBody _emailBody;
        private readonly IConfiguration _config;
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;
        private readonly HelperValidation _validation;
        private readonly INotificationService _notificationService;

        public EmployeeService(
            EmailBody emailBody,
            IConfiguration config,
            IRepository repository,
            IEmailService emailService,
            INotificationService notificationService,
            HelperValidation validation)
        {
            _config = config;
            _emailBody = emailBody;
            _repository = repository;
            _validation = validation;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public async Task<PageList<EmployeeDTO>> GetEmployeeAsync(PageParams pageParams, int companyId)
        {
            var query = _repository.Select<Employee>()
                .Include(p => p.Attachment)
                .Where(p => p.CompanyId == companyId && !p.IsRemoved &&
                            p.Name.ToLower().Contains(pageParams.Term.ToLower()))
                .OrderByDescending(p => p.CreationDate)
                .Select(p => new EmployeeDTO
                {
                    Id = p.Id,
                    CompanyId = p.CompanyId,
                    PositionId = p.PositionId,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    Amount = p.Amount,
                    Email = p.Email,
                    IsEmailConfirmed = p.IsEmailConfirmed,
                    EntryDate = p.EntryDate,
                    DepartureDate = p.DepartureDate,
                    DocumentNumber = p.DocumentNumber,
                    Gender = p.Gender,
                    Role = p.Role,
                    Status = p.Status,
                    PositionName = p.EmployeePosition.PositionName,
                    CompanyTime = p.CompanyTime,
                    CreationDate = p.CreationDate,
                    Attachment = p.Attachment != null ? new AttachmentDTO
                    {
                        Id = p.Attachment.Id,
                        EmployeeId = p.Id,
                        CompanyId = p.CompanyId,
                        Base64 = p.Attachment.Base64,
                        ImgUrl = "data:" + p.Attachment.ContentType + ";base64," + p.Attachment.Base64
                    } : null
                });

            var paginatedList = await PageList<EmployeeDTO>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);

            foreach (var employee in paginatedList)
            {
                Calculate(employee);
            }

            return paginatedList;
        }

        public async Task<EmployeeDTO> GetByIdAsync(int companyId, int id)
        {
            var employee = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && p.Id == id && !p.IsRemoved)
                .Select(p => new EmployeeDTO
                {
                    Id = p.Id,
                    CompanyId = p.CompanyId,
                    PositionId = p.PositionId,
                    PositionName = p.EmployeePosition.PositionName,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    Amount = p.Amount,
                    Email = p.Email,
                    EntryDate = p.EntryDate,
                    DepartureDate = p.DepartureDate,
                    DocumentNumber = p.DocumentNumber,
                    PhoneNumber = p.PhoneNumber,
                    CountryCode = p.CountryCode,
                    Role = p.Role,
                    Gender = p.Gender,
                    Status = p.Status,
                    Address = new EmployeeAddressDTO
                    {
                        Street = p.Address.Street,
                        HomeNumber = p.Address.HomeNumber,
                        Complement = p.Address.Complement,
                        District = p.Address.District,
                        City = p.Address.City,
                        State = p.Address.State,
                        Country = p.Address.Country,
                        ZipCode = p.Address.ZipCode
                    }
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            Calculate(employee);

            return employee;
        }

        public async Task CreateEmployeeAsync(int companyId, EmployeeAndAddressDTO createEmployee)
        {
            var company = await _repository.Select<Company>()
                .Where(p => p.Id == companyId && !p.IsRemoved)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (company == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            if (await _validation.CheckEmailExist(createEmployee.Employee.Email))
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.RegisterEmailError, HttpStatusCode.BadRequest);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            if (await _validation.CheckDocumentExist(createEmployee.Employee.DocumentNumber))
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.RegisterDocumentNumberError, HttpStatusCode.BadRequest);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var employee = new Employee
            {
                CompanyId = companyId,
                PositionId = createEmployee.Employee.PositionId,
                Name = createEmployee.Employee.Name,
                BirthDate = createEmployee.Employee.BirthDate,
                Amount = createEmployee.Employee.Amount,
                Email = createEmployee.Employee.Email,
                IsEmailConfirmed = false,
                EntryDate = createEmployee.Employee.EntryDate,
                DepartureDate = createEmployee.Employee.DepartureDate,
                DocumentNumber = createEmployee.Employee.DocumentNumber,
                PhoneNumber = createEmployee.Employee.PhoneNumber,
                CountryCode = createEmployee.Employee.CountryCode,
                Gender = createEmployee.Employee.Gender,
                Role = UserRoleType.User,
                IsRemoved = false,
                Address = new EmployeeAddress
                {
                    Street = createEmployee.Address.Street,
                    HomeNumber = createEmployee.Address.HomeNumber,
                    Complement = createEmployee.Address.Complement,
                    District = createEmployee.Address.District,
                    City = createEmployee.Address.City,
                    State = createEmployee.Address.State,
                    Country = createEmployee.Address.Country,
                    ZipCode = createEmployee.Address.ZipCode,
                    IsRemoved = false,
                }
            };

            if (employee.DepartureDate == null)
            {
                employee.Status = EmployeeStatusType.Active;
            }
            else
            {
                employee.Status = EmployeeStatusType.Disconnected;
            }

            _repository.Insert(employee);
            await _repository.SaveAsync();

            if (employee.Status == EmployeeStatusType.Active)
            {
                await SendEmailToEndRegister(employee);
            }
        }

        public async Task UpdateEmployeeAsync(int companyId, int id, EmployeeAndAddressDTO updateEmployee, UserRoleType userRole)
        {
            var employee = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && p.Id == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            employee.CompanyId = companyId;
            employee.Name = updateEmployee.Employee.Name;
            employee.BirthDate = updateEmployee.Employee.BirthDate;
            employee.DocumentNumber = updateEmployee.Employee.DocumentNumber;
            employee.PhoneNumber = updateEmployee.Employee.PhoneNumber;
            employee.CountryCode = updateEmployee.Employee.CountryCode;
            employee.Gender = updateEmployee.Employee.Gender;

            if (userRole == UserRoleType.Admin)
            {
                employee.Amount = updateEmployee.Employee.Amount;
                employee.EntryDate = updateEmployee.Employee.EntryDate;
                employee.DepartureDate = updateEmployee.Employee.DepartureDate;
                employee.PositionId = updateEmployee.Employee.PositionId;
                employee.Status = updateEmployee.Employee.Status ?? employee.Status;
                employee.Email = updateEmployee.Employee.Email;

                if (updateEmployee.Employee.DepartureDate == null)
                {
                    employee.Status = EmployeeStatusType.Active;
                }
                else
                {
                    employee.Status = EmployeeStatusType.Disconnected;
                }
            }

            //Address
            employee.Address.Street = updateEmployee.Address.Street;
            employee.Address.HomeNumber = updateEmployee.Address.HomeNumber;
            employee.Address.Complement = updateEmployee.Address.Complement;
            employee.Address.District = updateEmployee.Address.District;
            employee.Address.City = updateEmployee.Address.City;
            employee.Address.State = updateEmployee.Address.State;
            employee.Address.Country = updateEmployee.Address.Country;
            employee.Address.ZipCode = updateEmployee.Address.ZipCode;

            await _repository.SaveAsync();
        }

        public async Task DeleteEmployeeAsync(int companyId, int id)
        {
            var employee = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && p.Id == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            employee.IsRemoved = true;
            employee.Address.IsRemoved = true;

            await _repository.SaveAsync();
        }

        private async Task SendEmailToEndRegister(Employee employee)
        {
            var companyName = await _repository.Select<Company>()
                .Where(p => p.Id == employee.CompanyId && !p.IsRemoved)
                .Select(p => p.Name)
                .FirstOrDefaultAsync();

            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            var subjectEmail = "Finalizar Cadastro";
            string? from = _config["EmailSettings:From"];
            var emailModel = new Email(employee.Email, subjectEmail, _emailBody.StringBodyEndRegisterEmployee(emailToken, employee.CompanyId, employee.Id, companyName));

            _emailService.SendEmail(emailModel);
        }

        private static void Calculate(EmployeeDTO employee)
        {
            DateTime startDate = employee.EntryDate;
            DateTime endDate = employee.DepartureDate ?? DateTime.Now;

            TimeSpan timeSpan = endDate - startDate;

            int totalDays = (int)Math.Ceiling(timeSpan.TotalDays);

            // Calcular anos e meses
            int totalMonths = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;
            int years = totalMonths / 12;
            int months = totalMonths % 12;

            string companyTime = "";

            if (years > 0 || months > 0)
            {
                companyTime = $"{(years > 0 ? $"{years} {(years != 1 ? "Anos" : "Ano")}" : "")}" +
                              $"{(months > 0 ? (years > 0 ? " e " : "") + $"{months} {(months != 1 ? "Meses" : "Mês")}" : "")}";
            }
            else
            {
                if (totalDays == 0)
                {
                    totalDays = 1;
                    companyTime = $"{totalDays} {(totalDays != 1 ? "Dias" : "Dia")}";
                }
                else
                {
                    companyTime = $"{totalDays} {(totalDays != 1 ? "Dias" : "Dia")}";
                }
            }
            employee.CompanyTime = companyTime;
        }
    }
}
