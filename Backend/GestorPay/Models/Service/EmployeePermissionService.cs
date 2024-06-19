using GestorPay.Context;
using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GestorPay.Models.Service
{
    public class EmployeePermissionService : IEmployeePermissionService
    {
        private readonly IRepository _repository;
        private readonly INotificationService _notificationService;

        public EmployeePermissionService(IRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }
        public async Task<PageList<EmployeePermissionDTO>> GetEmployeesPermissionsAsync(PageParams pageParams, int companyId)
        {
            var query = _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && !p.IsRemoved && p.Status != EmployeeStatusType.Disconnected &&
                            p.Name.ToLower().Contains(pageParams.Term.ToLower()))
                .OrderByDescending(p => p.CreationDate)
                .Select(p => new EmployeePermissionDTO
                {
                    Id = p.Id,
                    CompanyId = p.CompanyId,
                    Name = p.Name,
                    Email = p.Email,
                    Role = p.Role,
                    DepartureDate = p.DepartureDate,
                    EntryDate = p.EntryDate,
                    CompanyTime = p.CompanyTime,
                    EmployeePosition = p.EmployeePosition.PositionName,
                    CreationDate = p.CreationDate
                });

            var paginatedList = await PageList<EmployeePermissionDTO>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);

            foreach (var employee in paginatedList)
            {
                Calculate(employee);
            }

            return paginatedList;
        }

        public async Task<EmployeePermissionDTO> GetEmployeesPermissionsByIdAsync(int companyId, int employeeId)
        {
            var permission = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && p.Id == employeeId &&
                            !p.IsRemoved && p.Status != EmployeeStatusType.Disconnected)
                .Select(p => new EmployeePermissionDTO
                {
                    Id = p.Id,
                    CompanyId = p.CompanyId,
                    Name = p.Name,
                    Email = p.Email,
                    Role = p.Role,
                    EntryDate = p.EntryDate,
                    DepartureDate = p.DepartureDate,
                    EmployeePosition = p.EmployeePosition.PositionName,
                    Address = p.Address.District + ", " + p.Address.Street +
                                " " + p.Address.HomeNumber + ", " + p.Address.City +
                                "-" + p.Address.State + ", " + p.Address.Country
                })
                .FirstOrDefaultAsync();

            Calculate(permission);

            return permission;
        }

        public async Task UpdateEmployeePermissionAsync(int companyId, int Id, UserRoleType userRole)
        {
            var permission = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && p.Id == Id &&
                            !p.IsRemoved && p.Status != EmployeeStatusType.Disconnected)
                .FirstOrDefaultAsync();

            if (permission == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            permission.Role = userRole;

            await _repository.SaveAsync();
        }

        private static void Calculate(EmployeePermissionDTO employee)
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
