namespace GestorPay.Models.DTOs
{
    public class EmployeeDashboardDTO
    {
        public string EmployeeWithHighestAmountName { get; set; }
        public decimal EmployeeWithHighestAmount { get; set; }
        public string EmployeeWithHighestAmountPosition { get; set; }
        public string LastEmployeeHiredName { get; set; }
        public string LastEmployeeHiredPosition { get; set; }
        public DateTime LastEmployeeHiredDate { get; set; }
        public int QuantityEmployeesActive { get; set; }
        public decimal TotalAmountEmployees { get; set; }
        public List<AccumulatedExpensesDashboard> AccumulatedExpenses { get; set; }
        public List<SpendingDashboard> TotalSpendingAmount { get; set; }
        public List<ExpensesDashboard> MonthlyExpense { get; set; }
    }

    public class ExpensesDashboard
    {
        public decimal EmployeeAmount { get; set; }
        public int MonthNumber { get; set; }
    }

    public class SpendingDashboard
    {
        public decimal Amount { get; set; }
        public int MonthNumber { get; set; }
    }

    public class AccumulatedExpensesDashboard
    {
        public int MonthNumber { get; set; }
        public decimal AccumulatedAmount { get; set; }
    }
}
