using GestorPay.Context;
using GestorPay.Enumerator;
using GestorPay.Helper;
using GestorPay.Models.DTOs;
using GestorPay.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GestorPay.Models.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository _repository;
        private readonly INotificationService _notificationService;

        public DashboardService(IRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }
        public async Task<EmployeeDashboardDTO> GetEmployeeDashboardAsync(int companyId)
        {
            var employees = await _repository.Select<Employee>()
                .Where(p => p.CompanyId == companyId &&
                            p.Status == EmployeeStatusType.Active &&
                            !p.IsRemoved)
                .ToListAsync();

            if (employees == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var month = await GraphicCard(employees);
            var spendingAmount = await SpendingManagers(companyId);
            var accumulatedMonthlyExpenses = await GetAccumulatedMonthlyExpensesAsync(employees);

            var lastEmployeeHired = employees.OrderByDescending(p => p.EntryDate).FirstOrDefault();
            var employeeWithHighestAmount = employees.OrderByDescending(p => p.Amount).FirstOrDefault();
            var quantityEmployeesActive = employees.Count();

            var totalEmployeesAmount = employees.Sum(p => p.Amount);

            if (lastEmployeeHired == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            if (employeeWithHighestAmount == null)
            {
                var validationMessage = _notificationService.GetValidationMessage(ValidationType.EmployeeNotFound, HttpStatusCode.NotFound);
                throw new CustomException(validationMessage.Message, validationMessage.StatusCode);
            }

            var dashboard = new EmployeeDashboardDTO();

            dashboard.EmployeeWithHighestAmountName = employeeWithHighestAmount.Name;
            dashboard.EmployeeWithHighestAmount = employeeWithHighestAmount.Amount;
            dashboard.EmployeeWithHighestAmountPosition = employeeWithHighestAmount.EmployeePosition.PositionName;
            dashboard.LastEmployeeHiredName = lastEmployeeHired.Name;
            dashboard.LastEmployeeHiredPosition = lastEmployeeHired.EmployeePosition.PositionName;
            dashboard.LastEmployeeHiredDate = lastEmployeeHired.EntryDate;
            dashboard.QuantityEmployeesActive = quantityEmployeesActive;
            dashboard.MonthlyExpense = month;
            dashboard.TotalSpendingAmount = spendingAmount;
            dashboard.TotalAmountEmployees = totalEmployeesAmount;
            dashboard.AccumulatedExpenses = accumulatedMonthlyExpenses;

            return dashboard;
        }

        public async Task<List<ExpensesDashboard>> GraphicCard(List<Employee> employees)
        {
            var year = DateTime.Now.Year;

            var monthExpensesEmployee = employees.Where(p => p.CreationDate.Year == year)
                    .OrderBy(p => p.CreationDate)
                    .GroupBy(p => p.CreationDate.Month)
                    .Select(p => new ExpensesDashboard
                    {
                        MonthNumber = p.Key,
                        EmployeeAmount = p.Sum(a => a.Amount)
                    })
                    .ToList();

            var monthly = new List<ExpensesDashboard>();
            for (int i = 1; i <= 12; i++)
            {
                var expense = monthExpensesEmployee.FirstOrDefault(p => p.MonthNumber == i);
                monthly.Add(new ExpensesDashboard
                {
                    MonthNumber = i,
                    EmployeeAmount = expense?.EmployeeAmount ?? 0,
                });
            }

            return monthly;
        }

        public async Task<List<SpendingDashboard>> SpendingManagers(int companyId)
        {
            var year = DateTime.Now.Year;
            var spendingManagers = await _repository.Select<SpendingManager>()
                .Where(p => p.CompanyId == companyId && p.CreationDate.Year == year && p.Status == SpendingManagerStatusType.Active && !p.IsRemoved)
                .GroupBy(p => p.CreationDate.Month)
                .Select(p => new SpendingDashboard
                {
                    MonthNumber = p.Key,
                    Amount = p.Sum(a => a.Amount)
                })
                .ToListAsync();

            var spending = new List<SpendingDashboard>();
            for (int i = 1; i <= 12; i++)
            {
                var amountManager = spendingManagers.FirstOrDefault(p => p.MonthNumber == i);
                spending.Add(new SpendingDashboard
                {
                    MonthNumber = i,
                    Amount = amountManager?.Amount ?? 0
                });
            }

            return spending;
        }

        public async Task<List<AccumulatedExpensesDashboard>> GetAccumulatedMonthlyExpensesAsync(List<Employee> employeesList)
        {
            var year = DateTime.Now.Year;

            var employees = employeesList.Where(p => p.CreationDate.Year == year).ToList();

            var monthExpensesEmployee = employees
                .GroupBy(p => p.CreationDate.Month)
                .OrderBy(p => p.Key)
                .ToList();

            var accumulatedMonthlyExpenses = new List<AccumulatedExpensesDashboard>();
            decimal accumulatedAmount = 0;

            for (int i = 1; i <= 12; i++)
            {
                var currentMonthExpense = monthExpensesEmployee.FirstOrDefault(p => p.Key == i)?.Sum(a => a.Amount) ?? 0;
                accumulatedAmount += currentMonthExpense;

                accumulatedMonthlyExpenses.Add(new AccumulatedExpensesDashboard
                {
                    MonthNumber = i,
                    AccumulatedAmount = accumulatedAmount
                });
            }

            return accumulatedMonthlyExpenses;
        }


    }
}
