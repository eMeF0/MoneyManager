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
                .ToListAsync(cancellationToken);

            return View(categories);
        }

        // GET: /Category/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var categories = await _appDbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        public async Task<IActionResult> Create(Category model, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            _appDbContext.Categories.Add(model);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return RedirectToAction("GetAll");
        }
    }
}
