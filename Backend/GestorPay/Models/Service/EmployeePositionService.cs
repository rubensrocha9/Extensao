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
    public class EmployeePositionService : IEmployeePositionService
    {
        private readonly IRepository _repository;
        private readonly INotificationService _notificationService;

        public EmployeePositionService(IRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<PageList<EmployeePositionDTO>> GetEmployeePositionsAsync(int companyId, PageParams pageParams)
        {
            var query = _repository.Select<EmployeePosition>()
                .Where(p => p.CompanyId == companyId && !p.IsRemoved &&
                            p.PositionName.ToLower().Contains(pageParams.Term.ToLower()))
                .OrderByDescending(p => p.CreationDate)
                .Select(p => new EmployeePositionDTO
                {
                    Id = p.Id,
                    PositionName = p.PositionName,
                    CreationDate = p.CreationDate
                });

            return await PageList<EmployeePositionDTO>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }

        public async Task<List<EmployeePositionDTO>> GetListPositionsAsync(int companyId)
        {
            return await _repository.Select<EmployeePosition>()
                .Where(p => p.CompanyId == companyId && !p.IsRemoved)
                .Select(p => new EmployeePositionDTO
                {
                    Id = p.Id,
                    PositionName = p.PositionName,
                    CreationDate = p.CreationDate
                })
                .ToListAsync();
        }

        public async Task<EmployeePositionDTO> GetEmployeePositionByIdAsync(int companyId, int id)
        {
            var position = await _repository.Select<EmployeePosition>()
                .Where(p => p.CompanyId == companyId && p.Id == id && !p.IsRemoved)
                .Select(p => new EmployeePositionDTO
                {
                    Id = p.Id,
                    PositionName = p.PositionName
                })
                .FirstOrDefaultAsync();

            if (position == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeePositionNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            return position;
        }

        public async Task CreateEmployeePositionAsync(int companyId, EmployeePositionDTO employeePosition)
        {
            var company = await _repository.Select<Company>()
                .Where(p => p.Id == companyId && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            EmployeePosition position = new EmployeePosition();

            position.CompanyId = companyId;
            position.PositionName = employeePosition.PositionName;
            position.IsRemoved = false;

            _repository.Insert(position);
            await _repository.SaveAsync();
        }

        public async Task UpdateEmployeePositionAsync(int companyId, int id, EmployeePositionDTO employeePosition)
        {
            var position = await _repository.Select<EmployeePosition>()
                .Where(p => p.CompanyId == companyId && p.Id == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (position == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeePositionNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            position.PositionName = employeePosition.PositionName;
            position.IsRemoved = false;

            await _repository.SaveAsync();
        }

        public async Task DeleteEmployeePositionAsync(int companyId, int id)
        {
            var position = await _repository.Select<EmployeePosition>()
                .Where(p => p.CompanyId == companyId && p.Id == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            var employee = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && !p.IsRemoved && p.PositionId == id)
                .FirstOrDefaultAsync();

            if (position == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeePositionNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            if (employee != null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.PositionInUse, HttpStatusCode.BadRequest);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            position.IsRemoved = true;
            await _repository.SaveAsync();
        }

    }
}
