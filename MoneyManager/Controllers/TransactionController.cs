using Microsoft.AspNetCore.Mvc;

namespace MoneyManager.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
