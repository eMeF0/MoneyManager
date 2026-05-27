using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Data;
using MoneyManager.Models;
using MoneyManager.ViewModels;

namespace MoneyManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var transactions = await _appDbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync(cancellationToken);

            var categoriesCount = await _appDbContext.Categories
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var now = DateTime.Today;
            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-5);

            var monthlyBalance = Enumerable.Range(0, 6)
                .Select(offset => monthStart.AddMonths(offset))
                .Select(month =>
                {
                    var monthTransactions = transactions
                        .Where(t => t.Date.Year == month.Year && t.Date.Month == month.Month)
                        .ToList();

                    return new MonthlyBalancePoint
                    {
                        Label = month.ToString("MMM yyyy"),
                        Income = monthTransactions
                            .Where(t => t.Type == TransactionType.Income)
                            .Sum(t => t.Amount),
                        Expenses = monthTransactions
                            .Where(t => t.Type == TransactionType.Expense)
                            .Sum(t => t.Amount)
                    };
                })
                .ToList();

            var viewModel = new DashboardViewModel
            {
                TotalIncome = transactions
                    .Where(t => t.Type == TransactionType.Income)
                    .Sum(t => t.Amount),
                TotalExpenses = transactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount),
                TransactionCount = transactions.Count,
                CategoryCount = categoriesCount,
                MonthlyBalance = monthlyBalance,
                SpendingByCategory = transactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .GroupBy(t => t.Category?.Name ?? "Uncategorized")
                    .Select(group => new CategorySpendPoint
                    {
                        Category = group.Key,
                        Amount = group.Sum(t => t.Amount)
                    })
                    .OrderByDescending(x => x.Amount)
                    .Take(6)
                    .ToList(),
                RecentTransactions = transactions.Take(8).ToList()
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
