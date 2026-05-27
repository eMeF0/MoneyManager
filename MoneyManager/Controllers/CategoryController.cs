using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Data;
using MoneyManager.Models;

namespace MoneyManager.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GET: /Category
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await _appDbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            return View(categories);
        }

        // GET: /Category/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var categories = await _appDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            _appDbContext.Categories.Add(model);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var category = await _appDbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model, CancellationToken cancellationToken)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = await _appDbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            category.Description = model.Description;

            await _appDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var category = await _appDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var category = await _appDbContext.Categories
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                return NotFound();
            }

            if (category.Transactions.Any())
            {
                ModelState.AddModelError(string.Empty, "This category has transactions and cannot be deleted.");
                return View(category);
            }

            _appDbContext.Categories.Remove(category);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
