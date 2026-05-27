using MoneyManager.Models;

namespace MoneyManager.ViewModels;

public class DashboardViewModel
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Balance => TotalIncome - TotalExpenses;
    public int TransactionCount { get; set; }
    public int CategoryCount { get; set; }
    public IReadOnlyList<MonthlyBalancePoint> MonthlyBalance { get; set; } = [];
    public IReadOnlyList<CategorySpendPoint> SpendingByCategory { get; set; } = [];
    public IReadOnlyList<Transaction> RecentTransactions { get; set; } = [];
}

public class MonthlyBalancePoint
{
    public string Label { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
}

public class CategorySpendPoint
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
