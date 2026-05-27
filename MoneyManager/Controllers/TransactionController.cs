using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.Models;
using MoneyManager.ViewModels;

namespace MoneyManager.Controllers
{
    public class TransactionController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TransactionController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var transactions = await _appDbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync(cancellationToken);

            return View(transactions);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var transaction = await _appDbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var viewModel = new TransactionFormViewModel
            {
                Categories = await GetCategoryOptions(cancellationToken)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionFormViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!await CategoryExists(viewModel.CategoryId, cancellationToken))
            {
                ModelState.AddModelError(nameof(viewModel.CategoryId), "Choose an existing category.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.Categories = await GetCategoryOptions(cancellationToken);
                return View(viewModel);
            }

            var transaction = new Transaction
            {
                Amount = viewModel.Amount,
                Date = viewModel.Date,
                Type = viewModel.Type,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId
            };

            _appDbContext.Transactions.Add(transaction);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var transaction = await _appDbContext.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            var viewModel = new TransactionFormViewModel
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                Categories = await GetCategoryOptions(cancellationToken)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TransactionFormViewModel viewModel, CancellationToken cancellationToken)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!await CategoryExists(viewModel.CategoryId, cancellationToken))
            {
                ModelState.AddModelError(nameof(viewModel.CategoryId), "Choose an existing category.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.Categories = await GetCategoryOptions(cancellationToken);
                return View(viewModel);
            }

            var transaction = await _appDbContext.Transactions
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = viewModel.Amount;
            transaction.Date = viewModel.Date;
            transaction.Type = viewModel.Type;
            transaction.Description = viewModel.Description;
            transaction.CategoryId = viewModel.CategoryId;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var transaction = await _appDbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var transaction = await _appDbContext.Transactions
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            _appDbContext.Transactions.Remove(transaction);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Categories.AnyAsync(c => c.Id == id, cancellationToken);
        }

        private async Task<List<SelectListItem>> GetCategoryOptions(CancellationToken cancellationToken)
        {
            return await _appDbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToListAsync(cancellationToken);
        }
    }
}
