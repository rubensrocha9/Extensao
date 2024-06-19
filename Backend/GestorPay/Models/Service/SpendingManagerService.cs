using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.DTOs;
using GestorPay.Models.Helper;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GestorPay.Models.Service
{
    public class SpendingManagerService : ISpendingManagerService
    {
        private readonly IRepository _repository;
        private readonly INotificationService _notificationService;

        public SpendingManagerService(IRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task<PageList<SpendingManagerDTO>> GetSpendingManagerAsync(int companyId, PageParams pageParams)
        {
            var totalAmount = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId && !p.IsRemoved)
                .SumAsync(e => e.Amount);

            if (totalAmount != 0)
            {
                await CreateExpenseManagementEmployee(companyId, totalAmount);
            }

            var query = _repository.Select<SpendingManager>()
                .Where(p => p.CompanyId == companyId && !p.IsRemoved &&
                            p.Name.ToLower().Contains(pageParams.Term.ToLower()))
                .OrderByDescending(p => p.CreationDate)
                .Select(p => new SpendingManagerDTO
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    CompanyId = companyId,
                    Name = p.Name,
                    Status = p.Status,
                    CreationDate = p.CreationDate,
                    IsExpenseEmployee = p.IsExpenseEmployee
                });

            var paginatedList = await PageList<SpendingManagerDTO>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);

            return paginatedList;
        }

        public async Task<SpendingManagerDTO> GetSpendingManagerByIdAsync(int id, int companyId)
        {
            var company = await _repository.Select<Company>()
                .Where(p => p.Id == companyId && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var spendingManagers = await _repository.Select<SpendingManager>()
                .Where(p => p.CompanyId == company.Id && p.Id == id && !p.IsRemoved)
                .Select(p => new SpendingManagerDTO
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    CompanyId = company.Id,
                    Name = p.Name,
                    Status = p.Status
                })
                .FirstOrDefaultAsync();

            return spendingManagers;
        }

        public async Task CreateSpendingManagerAsync(int companyId, SpendingManagerDTO createSpending)
        {
            var company = await _repository.Select<Company>()
                .Where(p => p.Id == companyId)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            SpendingManager spendingManager = new SpendingManager();

            spendingManager.CompanyId = companyId;
            spendingManager.Name = createSpending.Name;
            spendingManager.Amount = createSpending.Amount;
            spendingManager.Status = createSpending.Status;
            spendingManager.IsExpenseEmployee = false;
            spendingManager.IsRemoved = false;

            _repository.Insert(spendingManager);
            await _repository.SaveAsync();
        }

        public async Task UpdateSpendingManagerAsync(int companyId, int id, SpendingManagerDTO updateSpending)
        {
            var company = await _repository.Select<Company>()
                .Where(p => p.Id == companyId && !p.IsRemoved)
                .AnyAsync();

            if (!company)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.CompanyNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var spendingManager = await _repository.Select<SpendingManager>()
                .Where(p => p.Id == id && !p.IsRemoved && p.CompanyId == companyId)
                .FirstOrDefaultAsync();

            if (spendingManager == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.SpendingManagerNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            spendingManager.Name = updateSpending.Name;
            spendingManager.Amount = updateSpending.Amount;
            spendingManager.Status = updateSpending.Status;

            await _repository.SaveAsync();
        }

        public async Task DeleteSpendingManagerAsync(int companyId, int id)
        {
            var spending = await _repository.Select<SpendingManager>()
                .Where(p => p.CompanyId == companyId && p.Id == id && !p.IsRemoved)
                .FirstOrDefaultAsync();

            if (spending == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.SpendingManagerNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            spending.IsRemoved = true;
            await _repository.SaveAsync();
        }

        private async Task CreateExpenseManagementEmployee(int companyId, decimal totalAmount)
        {
            var expenseEmployee = await _repository.Select<SpendingManager>()
                .Where(p => p.CompanyId == companyId && p.IsExpenseEmployee && !p.IsRemoved)
                .Select(p => new
                {
                    p.Id,
                    p.Amount
                })
                .FirstOrDefaultAsync();

            var amount = expenseEmployee != null ? expenseEmployee.Amount : 0;

            if (amount != totalAmount)
            {
                if (expenseEmployee == null)
                {
                    var spendingEmployee = new SpendingManager();
                    spendingEmployee.CompanyId = companyId;
                    spendingEmployee.Name = "Funcionários";
                    spendingEmployee.Amount = totalAmount;
                    spendingEmployee.Status = SpendingManagerStatusType.Active;
                    spendingEmployee.IsExpenseEmployee = true;
                    spendingEmployee.IsRemoved = false;

                    _repository.Insert(spendingEmployee);
                    await _repository.SaveAsync();
                }
                else
                {
                    var spendingManager = await _repository.Select<SpendingManager>()
                        .Where(p => p.Id == expenseEmployee.Id && !p.IsRemoved && p.CompanyId == companyId)
                        .FirstOrDefaultAsync();

                    spendingManager.Amount = totalAmount;
                    await _repository.SaveAsync();
                }
            }
        }
    }
}
